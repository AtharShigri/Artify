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

        public ArtworkService(
            IArtworkRepository artworkRepo,
            IArtistRepository artistRepo)
        {
            _artworkRepo = artworkRepo;
            _artistRepo = artistRepo;
        }

        public async Task<object> GetAllAsync(ClaimsPrincipal user)
        {
            var artistId = _artistRepo.GetArtistId(user);
            var artworks = await _artworkRepo.GetAllByArtistAsync(artistId);

            return artworks.Select(a => new
            {
                a.ArtworkId,
                a.Title,
                a.Description,
                a.Price,
                a.CategoryEntity,
                a.ImageUrl,
                a.IsForSale
            });
        }

        public async Task<object> GetByIdAsync(ClaimsPrincipal user, Guid artworkId)
        {
            var artistId = _artistRepo.GetArtistId(user);
            var artwork = await _artworkRepo.GetByIdAsync(artworkId);

            if (artwork == null || artwork.ArtistProfileId != artistId)
                return null;

            return new
            {
                artwork.ArtworkId,
                artwork.Title,
                artwork.Description,
                artwork.Price,
                artwork.CategoryEntity,
                artwork.ImageUrl,
                artwork.IsForSale
            };
        }

        public async Task<object> UploadAsync(ClaimsPrincipal user, ArtworkUploadDto dto)
        {
            var artistId = _artistRepo.GetArtistId(user);

            // Save file
            var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
            var path = Path.Combine("wwwroot/images/artworks", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var artwork = new Artwork
            {
                ArtistProfileId = artistId,
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                CategoryEntity = dto.Category,
                ImageUrl = $"/images/artworks/{fileName}",
                IsForSale = true
            };

            await _artworkRepo.AddAsync(artwork);
            return new { Success = true, ArtworkId = artwork.ArtworkId, ArtworkUrl = artwork.ImageUrl };
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
            artwork.CategoryEntity = dto.Category ?? artwork.CategoryEntity;
            artwork.IsForSale = dto.IsAvailable ?? artwork.IsForSale;

            await _artworkRepo.UpdateAsync(artwork);
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
