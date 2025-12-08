using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IArtServiceRepository
    {
        Task<IEnumerable<ArtService>> GetAllByArtistAsync(int artistId);
        Task<ArtService> GetByIdAsync(int serviceId);

        Task AddAsync(ArtService service);
        Task UpdateAsync(ArtService service);
        Task DeleteAsync(ArtService service);

        Task<bool> ServiceExistsAsync(int serviceId, int artistId);
    }
}
