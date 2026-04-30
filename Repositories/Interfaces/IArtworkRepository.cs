using artifi.Api.Models;

namespace artifi.Api.Repositories.Interfaces
{
    public interface IArtworkRepository
    {
        Task<IEnumerable<Artwork>> GetAllByArtistAsync(Guid artistId);
        Task<Artwork> GetByIdAsync(Guid artworkId);

        Task AddAsync(Artwork artwork);
        Task UpdateAsync(Artwork artwork);
        Task DeleteAsync(Artwork artwork);

        Task<bool> ArtworkExistsAsync(Guid artworkId, Guid artistId);
        Task<IEnumerable<Artwork>> GetArtworksByArtistIdsAsync(IEnumerable<Guid> artistIds);
        Task<Category?> GetCategoryByNameAsync(string name);
    }
}
