using artifi.Api.DTOs.Artist;
using System.Security.Claims;

namespace artifi.Api.Services.Interfaces
{
    public interface IProtectionService
    {
        Task<WatermarkResultDto> ApplyWatermarkAsync(ClaimsPrincipal user, IFormFile file);
        Task<MetadataResultDto> EmbedMetadataAsync(ClaimsPrincipal user, MetadataDto dto);
        Task<HashResultDto> GenerateHashAsync(ClaimsPrincipal user, HashDto dto);
        Task<PlagiarismResultDto> CheckPlagiarismAsync(ClaimsPrincipal user, byte[] imageBytes);
        Task<ProtectionStatusDto?> GetProtectionStatusAsync(ClaimsPrincipal user, Guid artworkId);
    }
}
