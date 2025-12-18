using Artify.Api.DTOs.Buyer;
using Artify.Api.DTOs.Shared;
using Artify.Api.Models;

namespace Artify.Api.Services.Interfaces
{
    public interface IBuyerService
    {
        // Buyer Profile
        Task<BuyerProfileResponseDto?> GetBuyerProfileAsync(string buyerId);
        Task<BuyerProfileResponseDto?> UpdateBuyerProfileAsync(string buyerId, BuyerUpdateDto updateDto);
        Task<bool> DeleteBuyerAccountAsync(string buyerId);

        // Marketplace Browsing
        Task<IEnumerable<ArtworkResponseDto>> GetFeaturedArtworksAsync();
        Task<IEnumerable<ArtworkResponseDto>> GetArtworksByCategoryAsync(Category category);
/*        Task<IEnumerable<ArtworkResponseDto>> SearchArtworksAsync(string query, Category? category = null);
*/        Task<ArtworkDetailDto?> GetArtworkDetailAsync(Guid artworkId);
        Task<ArtistProfileDto?> GetArtistProfileAsync(Guid artistProfileId);

        // Statistics
        Task<int> GetTotalOrdersAsync(string buyerId);
        Task<int> GetTotalReviewsAsync(string buyerId);
    }
}