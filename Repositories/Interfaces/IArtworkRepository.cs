using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IArtworkRepository
    {
        Task<IEnumerable<Artwork>> GetAllByArtistAsync(int artistId);
        Task<Artwork> GetByIdAsync(Guid artworkId);

        Task AddAsync(Artwork artwork);
        Task UpdateAsync(Artwork artwork);
        Task DeleteAsync(Artwork artwork);

        Task<bool> ArtworkExistsAsync(Guid artworkId, int artistId);
    }
}
