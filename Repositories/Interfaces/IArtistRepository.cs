using System.Security.Claims;
using Artify.Api.DTOs.Artist;
using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IArtistRepository
    {
        Task<ApplicationUser> GetByIdAsync(Guid artistId);
        Task<ApplicationUser> GetByEmailAsync(string email);

        Task AddAsync(ApplicationUser artist);
        Task UpdateAsync(ApplicationUser artist);
        Task DeleteAsync(ApplicationUser artist);

        Guid GetArtistId(ClaimsPrincipal user);
    }
}
