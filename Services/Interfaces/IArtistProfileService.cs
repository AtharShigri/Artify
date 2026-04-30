using System.Security.Claims;
using artifi.Api.DTOs.Artist;
using artifi.Api.DTOs.Auth;

namespace artifi.Api.Services.Interfaces
{
    public interface IArtistProfileService
    {
        Task<object> GetProfileAsync(ClaimsPrincipal user);
        Task<object> UpdateProfileAsync(ClaimsPrincipal user, ArtistUpdateDto dto);
        Task<object> UpdateProfileImageAsync(ClaimsPrincipal user, IFormFile file);
        Task<object> DeleteProfileAsync(ClaimsPrincipal user);
    }
}
