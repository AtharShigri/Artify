using Artify.Api.DTOs.Buyer;
using Artify.Api.DTOs.Shared;

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
        Task<IEnumerable<ArtworkResponseDto>> GetArtworksByCategoryAsync(string category);
        Task<IEnumerable<ArtworkResponseDto>> SearchArtworksAsync(string query, string? category = null);
        Task<ArtworkDetailDto?> GetArtworkDetailAsync(int artworkId);
        Task<ArtistProfileDto?> GetArtistProfileAsync(int artistProfileId);

        // Statistics
        Task<int> GetTotalOrdersAsync(string buyerId);
        Task<int> GetTotalReviewsAsync(string buyerId);
    }
}