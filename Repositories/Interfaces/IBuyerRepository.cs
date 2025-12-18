using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IBuyerRepository
    {
        // Buyer Profile Operations
        Task<ApplicationUser?> GetBuyerByIdAsync(string buyerId);
        Task<ApplicationUser?> GetBuyerByEmailAsync(string email);
        Task<bool> UpdateBuyerAsync(ApplicationUser buyer);
        Task<bool> DeleteBuyerAsync(string buyerId);

        // Marketplace Operations
        Task<IEnumerable<Artwork>> GetFeaturedArtworksAsync(int count = 10);
        Task<Artwork?> GetArtworkByIdAsync(Guid artworkId);
        Task<IEnumerable<Artwork>> GetArtworksByCategoryAsync(Category? category, int page = 1, int pageSize = 20);
        Task<IEnumerable<Artwork>> SearchArtworksAsync(string query, decimal? minPrice = null, decimal? maxPrice = null, string sortBy = "newest");
        Task<ArtistProfile?> GetArtistProfileByIdAsync(int artistProfileId);
        Task<IEnumerable<ArtistProfile>> GetFeaturedArtistsAsync(int count = 10);

        // Utility
        Task<bool> SaveChangesAsync();

       
        Task<bool> IncrementArtworkViewsAsync(Guid artworkId);

    }
}