using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;

namespace Artify.Api.Repositories.Implementations
{
    public class ArtworkRepository : IArtworkRepository
    {
        public Task AddAsync(Artwork artwork) => Task.CompletedTask;
        public Task DeleteAsync(Artwork artwork) => Task.CompletedTask;
        public Task<IEnumerable<Artwork>> GetAllByArtistAsync(int artistId) => Task.FromResult<IEnumerable<Artwork>>(null);
        public Task<Artwork> GetByIdAsync(Guid artworkId) => Task.FromResult<Artwork>(null);
        public Task UpdateAsync(Artwork artwork) => Task.CompletedTask;
        public Task<bool> ArtworkExistsAsync(Guid artworkId, int artistId) => Task.FromResult(false);
    }
}
