using Artify.Api.DTOs.Buyer;

namespace Artify.Api.Services.Interfaces
{
    public interface IReviewService
    {
        // Review Operations
        Task<ReviewResponseDto> CreateReviewAsync(string buyerId, ReviewDto reviewDto);
        Task<ReviewResponseDto> UpdateReviewAsync(int reviewId, string buyerId, ReviewDto reviewDto);
        Task<bool> DeleteReviewAsync(int reviewId, string buyerId);

        // Get Reviews
        Task<IEnumerable<ReviewResponseDto>> GetArtworkReviewsAsync(int artworkId);
        Task<IEnumerable<ReviewResponseDto>> GetArtistReviewsAsync(int artistProfileId);
        Task<ReviewResponseDto?> GetReviewByIdAsync(int reviewId);

        // Review Statistics
        Task<double> GetAverageRatingAsync(int? artworkId = null, int? artistProfileId = null);
        Task<int> GetReviewCountAsync(int? artworkId = null, int? artistProfileId = null);

        // Validation
        Task<bool> CanUserReviewAsync(string buyerId, int? artworkId, int? artistProfileId);
    }
}