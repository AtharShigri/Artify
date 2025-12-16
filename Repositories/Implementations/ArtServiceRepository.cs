using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;

namespace Artify.Api.Repositories.Implementations
{
    public class ArtServiceRepository : IArtServiceRepository
    {
        public Task AddAsync(ArtService service) => Task.CompletedTask;
        public Task DeleteAsync(ArtService service) => Task.CompletedTask;
        public Task<IEnumerable<ArtService>> GetAllByArtistAsync(int artistId) => Task.FromResult<IEnumerable<ArtService>>(null);
        public Task<ArtService> GetByIdAsync(int serviceId) => Task.FromResult<ArtService>(null);
        public Task UpdateAsync(ArtService service) => Task.CompletedTask;
        public Task<bool> ServiceExistsAsync(int serviceId, int artistId) => Task.FromResult(false);
    }
}
