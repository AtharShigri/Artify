using System.Security.Claims;
using Artify.Api.DTOs.Artist;

namespace Artify.Api.Services.Interfaces
{
    public interface IProtectionService
    {
        Task<object> ApplyWatermarkAsync(ClaimsPrincipal user, IFormFile file);
        Task<object> EmbedMetadataAsync(ClaimsPrincipal user, MetadataDto dto);
        Task<object> GenerateHashAsync(ClaimsPrincipal user, HashDto dto);
        Task<object> CheckPlagiarismAsync(ClaimsPrincipal user, IFormFile file);
    }
}
