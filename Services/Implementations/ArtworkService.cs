using System.Security.Claims;
using Artify.Api.DTOs.Artist;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Services.Implementations
{
    public class ArtworkService : IArtworkService
    {
        private readonly IArtworkRepository _artworkRepo;
        private readonly IArtistRepository _artistRepo;
        private readonly IWebHostEnvironment _environment;
        private readonly IProtectionService _protectionService;

        public ArtworkService(
            IArtworkRepository artworkRepo,
            IArtistRepository artistRepo,
            IWebHostEnvironment environment,
            IProtectionService protectionService)
        {
            _artworkRepo = artworkRepo;
            _artistRepo = artistRepo;
            _environment = environment;
            _protectionService = protectionService;
        }

        public async Task<object> GetAllAsync(ClaimsPrincipal user)
        {
            // If user is not authenticated, we return all artworks
            Guid artistId = (user?.Identity?.IsAuthenticated == true) ? _artistRepo.GetArtistId(user) : Guid.Empty;
            var artworks = await _artworkRepo.GetAllByArtistAsync(artistId);

            return artworks.Select(a => new
            {
                a.ArtworkId,
                a.Title,
                a.Description,
                a.Price,
                Category = a.CategoryEntity?.Name ?? "N/A",
                a.ImageUrl,
                a.IsForSale,
                a.CreatedAt,
                a.ArtistProfileId,
                ArtistName = a.ArtistProfile?.User?.FullName ?? "Unknown Artist"
            });
        }

        public async Task<object> GetByIdAsync(ClaimsPrincipal user, Guid artworkId)
        {
            var artwork = await _artworkRepo.GetByIdAsync(artworkId);

            if (artwork == null)
                return null;

            var protectionStatus = await _protectionService.GetProtectionStatusAsync(user, artworkId);

            // If it's a guest or if the artist owns it, they can see it. 
            // In fact, since it's a GET request, anyone can see it now as per requirement.
            return new
            {
                artwork.ArtworkId,
                artwork.Title,
                artwork.Description,
                artwork.Price,
                Category = artwork.CategoryEntity?.Name ?? "N/A",
                artwork.ImageUrl,
                artwork.IsForSale,
                artwork.CreatedAt,
                artwork.ArtistProfileId,
                ArtistName = artwork.ArtistProfile?.User?.FullName ?? "Unknown Artist",
                ProtectionStatus = protectionStatus
            };
        }

        public async Task<object> UploadAsync(ClaimsPrincipal user, ArtworkUploadDto dto)
{
    var artistId = _artistRepo.GetArtistId(user);

    if (artistId == Guid.Empty)
    {
        return new { Success = false, Message = "Artist profile not found. Please complete your profile first." };
    }

    if (dto.File == null || dto.File.Length == 0)
    {
        return new { Success = false, Message = "No image file provided." };
    }

    var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
    var folderPath = Path.Combine(_environment.ContentRootPath, "wwwroot", "images", "artworks");
    var filePath = Path.Combine(folderPath, fileName);

    Directory.CreateDirectory(folderPath);
    
    string imageUrl = $"/images/artworks/{fileName}";

    // Handle Watermarking
    if (dto.ApplyWatermark)
    {
        var watermarkResult = await _protectionService.ApplyWatermarkAsync(user, dto.File);
        if (watermarkResult.Success)
        {
            imageUrl = watermarkResult.WatermarkedUrl;
        }
        else 
        {
            // Fallback to original if watermarking fails
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }
        }
    }
    else 
    {
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await dto.File.CopyToAsync(stream);
        }
    }

    var artwork = new Artwork
    {
        ArtistProfileId = artistId,
        Title = dto.Title,
        Description = dto.Description,
        Price = dto.Price,
        Metadata = dto.Metadata,     
        ImageUrl = imageUrl,
        IsForSale = true,
        CategoryId  = dto.CategoryId,
        Status = "Published"
    };

    await _artworkRepo.AddAsync(artwork);

    // Handle Metadata & Fingerprinting after saving artwork
    if (dto.RegisterFingerprint)
    {
        await _protectionService.GenerateHashAsync(user, new HashDto { ArtworkId = artwork.ArtworkId });
    }

    if (!string.IsNullOrEmpty(dto.CopyrightText))
    {
        var artistProfile = await _artistRepo.GetByIdAsync(artistId);
        await _protectionService.EmbedMetadataAsync(user, new MetadataDto 
        { 
            ArtworkId = artwork.ArtworkId, 
            CopyrightText = dto.CopyrightText,
            ArtistName = artistProfile?.FullName ?? "Artify Artist",
            Description = dto.Description
        });
    }

    // Auto Plagiarism Check
    var plagiarismResult = await _protectionService.CheckPlagiarismAsync(user, dto.File);
    if (plagiarismResult.PlagiarismDetected)
    {
        artwork.Status = "Flagged";
        await _artworkRepo.UpdateAsync(artwork);
    }
    
    return new { 
        Success = true, 
        ArtworkId = artwork.ArtworkId, 
        ArtworkUrl = artwork.ImageUrl,
        PlagiarismDetected = plagiarismResult.PlagiarismDetected
    };
}
        public async Task<object> UpdateAsync(ClaimsPrincipal user, Guid artworkId, ArtworkUpdateDto dto)
        {
            var artistId = _artistRepo.GetArtistId(user);
            var artwork = await _artworkRepo.GetByIdAsync(artworkId);

            if (artwork == null || artwork.ArtistProfileId != artistId)
                return null;

            artwork.Title = dto.Title ?? artwork.Title;
            artwork.Description = dto.Description ?? artwork.Description;
            artwork.Price = dto.Price ?? artwork.Price;
            artwork.CategoryId = dto.CategoryId ?? artwork.CategoryId;
            artwork.IsForSale = dto.IsAvailable ?? artwork.IsForSale;
            artwork.Metadata = dto.Metadata ?? artwork.Metadata;

            // Optional image replacement
            if (dto.File != null && dto.File.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
                var folderPath = Path.Combine(_environment.ContentRootPath, "wwwroot", "images", "artworks");
                var filePath = Path.Combine(folderPath, fileName);

                Directory.CreateDirectory(folderPath);

                if (dto.ApplyWatermark == true)
                {
                    var watermarkResult = await _protectionService.ApplyWatermarkAsync(user, dto.File);
                    if (watermarkResult.Success)
                    {
                        artwork.ImageUrl = watermarkResult.WatermarkedUrl;
                    }
                    else
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await dto.File.CopyToAsync(stream);
                        }
                        artwork.ImageUrl = $"/images/artworks/{fileName}";
                    }
                }
                else
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.File.CopyToAsync(stream);
                    }
                    artwork.ImageUrl = $"/images/artworks/{fileName}";
                }
            }

            await _artworkRepo.UpdateAsync(artwork);

            // Handle Metadata & Fingerprinting
            if (dto.RegisterFingerprint == true)
            {
                await _protectionService.GenerateHashAsync(user, new HashDto { ArtworkId = artwork.ArtworkId });
            }

            if (!string.IsNullOrEmpty(dto.CopyrightText))
            {
                var artistProfile = await _artistRepo.GetByIdAsync(artistId);
                await _protectionService.EmbedMetadataAsync(user, new MetadataDto
                {
                    ArtworkId = artwork.ArtworkId,
                    CopyrightText = dto.CopyrightText,
                    ArtistName = artistProfile?.FullName ?? "Artify Artist",
                    Description = artwork.Description
                });
            }

            return new { Success = true };
        }

        public async Task<object> DeleteAsync(ClaimsPrincipal user, Guid artworkId)
        {
            var artistId = _artistRepo.GetArtistId(user);
            var artwork = await _artworkRepo.GetByIdAsync(artworkId);

            if (artwork == null || artwork.ArtistProfileId != artistId)
                return null;

            await _artworkRepo.DeleteAsync(artwork);
            return new { Success = true };
        }
    }
}
