using System.Security.Claims;
using Artify.Api.DTOs.Artist;
using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IArtistRepository
    {
        Task<Artist> GetByIdAsync(Guid artistId);
        Task<Artist> GetByEmailAsync(string email);

        Task AddAsync(Artist artist);
        Task UpdateAsync(Artist artist);
        Task DeleteAsync(Artist artist);

        Guid GetArtistId(ClaimsPrincipal user);
    }
}
