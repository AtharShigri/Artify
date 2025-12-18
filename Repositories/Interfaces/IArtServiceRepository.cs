using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IArtServiceRepository
    {
        Task<IEnumerable<ArtService>> GetAllByArtistAsync(Guid artistId);
        Task<ArtService> GetByIdAsync(int serviceId);

        Task AddAsync(ArtService service);
        Task UpdateAsync(ArtService service);
        Task DeleteAsync(ArtService service);

        Task<bool> ServiceExistsAsync(int serviceId, Guid artistId);
    }
}
