using System.Security.Claims;
using Artify.Api.DTOs.Artist;
using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IArtistRepository
    {
        Task<Artist> GetByIdAsync(int artistId);
        Task<Artist> GetByEmailAsync(string email);

        Task AddAsync(Artist artist);
        Task UpdateAsync(Artist artist);
        Task DeleteAsync(Artist artist);

        int GetArtistId(ClaimsPrincipal user);
    }
}
