using System.Security.Claims;
using artifi.Api.DTOs.Artist;

namespace artifi.Api.Services.Interfaces
{
    public interface IArtServiceListingService
    {
        Task<object> GetAllAsync(ClaimsPrincipal user);
        Task<object> GetByIdAsync(ClaimsPrincipal user, Guid serviceId);
        
        Task<object> CreateAsync(ClaimsPrincipal user, ArtServiceDto dto);
        Task<object> UpdateAsync(ClaimsPrincipal user, Guid serviceId, ArtServiceDto dto);
        Task<object> DeleteAsync(ClaimsPrincipal user, Guid serviceId);
    }
}
