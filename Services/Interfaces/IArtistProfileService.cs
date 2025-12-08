using System.Security.Claims;
using Artify.Api.DTOs.Artist;

namespace Artify.Api.Services.Interfaces
{
    public interface IArtistProfileService
    {
        Task<object> RegisterAsync(ArtistRegisterDto dto);
        Task<object> LoginAsync(LoginDto dto);

        Task<object> GetProfileAsync(ClaimsPrincipal user);
        Task<object> UpdateProfileAsync(ClaimsPrincipal user, ArtistUpdateDto dto);
        Task<object> UpdateProfileImageAsync(ClaimsPrincipal user, IFormFile file);
        Task<object> DeleteProfileAsync(ClaimsPrincipal user);
    }
}
