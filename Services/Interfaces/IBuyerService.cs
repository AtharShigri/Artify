using artifi.Api.DTOs.Buyer;
using artifi.Api.DTOs.Shared;
using artifi.Api.Models;

namespace artifi.Api.Services.Interfaces
{
    public interface IBuyerService
    {
        // Buyer Profile
        Task<BuyerProfileResponseDto?> GetBuyerProfileAsync(Guid buyerId);
        Task<BuyerProfileResponseDto?> UpdateBuyerProfileAsync(Guid buyerId, BuyerUpdateDto updateDto);
        Task<bool> DeleteBuyerAccountAsync(Guid buyerId);

        // Marketplace Browsing
        Task<IEnumerable<ArtworkResponseDto>> GetFeaturedArtworksAsync();
        Task<IEnumerable<ArtworkResponseDto>> GetArtworksByCategoryAsync(Category category);
/*        Task<IEnumerable<ArtworkResponseDto>> SearchArtworksAsync(string query, Category? category = null);
*/        Task<ArtworkDetailDto?> GetArtworkDetailAsync(Guid artworkId);
        Task<ArtistProfileDto?> GetArtistProfileAsync(Guid artistProfileId);

        // Statistics
        Task<int> GetTotalOrdersAsync(Guid buyerId);
        Task<int> GetTotalReviewsAsync(Guid buyerId);
        Task<IEnumerable<OrderResponseDto>> GetBuyerOrdersAsync(Guid buyerId);
    }
}