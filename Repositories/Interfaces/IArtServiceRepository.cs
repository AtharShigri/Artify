using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IArtServiceRepository
    {
        Task<IEnumerable<Service>> GetAllByArtistAsync(Guid artistId);
        Task<Service> GetByIdAsync(Guid serviceId);

        Task AddAsync(Service service);
        Task UpdateAsync(Service service);
        Task DeleteAsync(Service service);

        Task<bool> ServiceExistsAsync(Guid serviceId, Guid artistId);
    }
}
