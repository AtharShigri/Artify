using System.Security.Claims;
using artifi.Api.DTOs.Artist;

namespace artifi.Api.Services.Interfaces
{
    public interface IArtworkService
    {
        Task<object> GetAllAsync(ClaimsPrincipal user);
        Task<object> GetByIdAsync(ClaimsPrincipal user, Guid artworkId);

        Task<object> UploadAsync(ClaimsPrincipal user, ArtworkUploadDto dto);
        Task<object> UpdateAsync(ClaimsPrincipal user, Guid artworkId, ArtworkUpdateDto dto);
        Task<object> DeleteAsync(ClaimsPrincipal user, Guid artworkId);
    }
}
