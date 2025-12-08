using System.Security.Claims;
using Artify.Api.DTOs.Artist;

namespace Artify.Api.Services.Interfaces
{
    public interface IArtServiceListingService
    {
        Task<object> GetAllAsync(ClaimsPrincipal user);
        Task<object> GetByIdAsync(ClaimsPrincipal user, int serviceId);

        Task<object> CreateAsync(ClaimsPrincipal user, ArtServiceDto dto);
        Task<object> UpdateAsync(ClaimsPrincipal user, int serviceId, ArtServiceDto dto);
        Task<object> DeleteAsync(ClaimsPrincipal user, int serviceId);
    }
}
