using Artify.Api.Models;

public interface IAdminArtworkRepository
{
    Task<IEnumerable<Artwork>> GetAllArtworksAsync();
    Task<IEnumerable<Artwork>> GetPendingArtworksAsync();
    Task<Artwork?> GetArtworkByIdAsync(int artworkId);
    Task<Artwork> UpdateArtworkAsync(Artwork artwork);
    Task<bool> RemoveArtworkAsync(int artworkId);
}
