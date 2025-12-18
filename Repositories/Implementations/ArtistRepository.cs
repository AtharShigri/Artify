using System.Security.Claims;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;

namespace Artify.Api.Repositories.Implementations
{
    public class ArtistRepository : IArtistRepository
    {
        public Task AddAsync(Artist artist) => Task.CompletedTask;
        public Task DeleteAsync(Artist artist) => Task.CompletedTask;
        public Task<Artist> GetByEmailAsync(string email) => Task.FromResult<Artist>(null);
        public Task<Artist> GetByIdAsync(Guid artistId) => Task.FromResult<Artist>(null);
        public Task UpdateAsync(Artist artist) => Task.CompletedTask;
        public Guid GetArtistId(ClaimsPrincipal user) => Guid.Empty;
    }
}
