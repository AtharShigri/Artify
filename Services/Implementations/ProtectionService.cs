using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Artify.Api.DTOs.Artist;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Services.Implementations
{
    public class ProtectionService : IProtectionService
    {
        private readonly IProtectionRepository _protectionRepo;
        private readonly IArtworkRepository _artworkRepo;
        private readonly IArtistRepository _artistRepo;

        public ProtectionService(
            IProtectionRepository protectionRepo,
            IArtworkRepository artworkRepo,
            IArtistRepository artistRepo)
        {
            _protectionRepo = protectionRepo;
            _artworkRepo = artworkRepo;
            _artistRepo = artistRepo;
        }

        public async Task<object> ApplyWatermarkAsync(ClaimsPrincipal user, IFormFile file)
        {
            var artistId = _artistRepo.GetArtistId(user);

            // Save file temporarily
            var tempFile = Path.Combine("wwwroot/images/temp", $"{Guid.NewGuid()}_{file.FileName}");
            using (var stream = new FileStream(tempFile, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Call Python microservice
            var pythonResult = await CallPythonWatermarkServiceAsync(tempFile);

            return new { Success = true, WatermarkedUrl = pythonResult };
        }

        public async Task<object> EmbedMetadataAsync(ClaimsPrincipal user, MetadataDto dto)
        {
            var artistId = _artistRepo.GetArtistId(user);
            var artwork = await _artworkRepo.GetByIdAsync(dto.ArtworkId);

            if (artwork == null || artwork.ArtistProfileId != artistId) return null;

            var metadataLog = new ArtworkMetadataLog
            {
                ArtworkId = dto.ArtworkId,
                ArtistName = dto.ArtistName,
                CopyrightText = dto.CopyrightText,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _protectionRepo.AddMetadataLogAsync(metadataLog);

            return new { Success = true };
        }

        public async Task<object> GenerateHashAsync(ClaimsPrincipal user, HashDto dto)
        {
            var artistId = _artistRepo.GetArtistId(user);
            var artwork = await _artworkRepo.GetByIdAsync(dto.ArtworkId);

            if (artwork == null || artwork.ArtistProfileId != artistId) return null;

            // Compute hash of the image file
            using var md5 = MD5.Create();
            using var stream = new FileStream($"wwwroot{artwork.ImageUrl}", FileMode.Open, FileAccess.Read);
            var hashBytes = md5.ComputeHash(stream);
            var hashValue = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            var hashRecord = new ArtworkHash
            {
                ArtworkId = dto.ArtworkId,
                HashValue = hashValue,
                CreatedAt = DateTime.UtcNow
            };

            await _protectionRepo.AddHashRecordAsync(hashRecord);

            return new { Success = true, Hash = hashValue };
        }

        public async Task<object> CheckPlagiarismAsync(ClaimsPrincipal user, IFormFile file)
        {
            var artistId = _artistRepo.GetArtistId(user);

            // Save file temporarily
            var tempFile = Path.Combine("wwwroot/images/temp", $"{Guid.NewGuid()}_{file.FileName}");
            using (var stream = new FileStream(tempFile, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var plagiarismResultString = await CallPythonPlagiarismServiceAsync(tempFile);

            double plagiarismResult = double.TryParse(
                plagiarismResultString,
                out var score)
                ? score
                : 0.0;

            var log = new PlagiarismLog
            {
                ArtworkId = Guid.Empty,             
                SuspectedArtworkId = Guid.NewGuid(),
                SimilarityScore = plagiarismResult,
                IsReviewed = false,
                ActionTaken = false,
                CreatedAt = DateTime.UtcNow
            };

            await _protectionRepo.AddPlagiarismLogAsync(log);

            return new
            {
                Success = true,
                Result = plagiarismResult
            };
        }


        // Dummy placeholders for calling Python microservices
        private Task<string> CallPythonWatermarkServiceAsync(string filePath)
        {
            // TODO: Replace with actual HTTP call to Python service
            var url = $"/images/watermarked/{Path.GetFileName(filePath)}";
            return Task.FromResult(url);
        }

        private Task<string> CallPythonPlagiarismServiceAsync(string filePath)
        {
            // TODO: Replace with actual HTTP call to Python service
            return Task.FromResult("No plagiarism detected");
        }
    }
}
