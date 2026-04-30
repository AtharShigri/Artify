using System.Security.Claims;
using artifi.Api.DTOs.Artist;
using artifi.Api.Models;

namespace artifi.Api.Repositories.Interfaces
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
