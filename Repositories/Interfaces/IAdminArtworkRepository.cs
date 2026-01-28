using Artify.Api.Models;

public interface IAdminArtworkRepository
{
    Task<IEnumerable<Artwork>> GetAllArtworksAsync();
    Task<IEnumerable<Artwork>> GetPendingArtworksAsync();
    Task<Artwork?> GetArtworkByIdAsync(Guid artworkId);
    Task<Artwork> UpdateArtworkAsync(Artwork artwork);
    Task<bool> RemoveArtworkAsync(Guid artworkId);
}
