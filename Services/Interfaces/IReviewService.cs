using Artify.Api.DTOs.Buyer;

namespace Artify.Api.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDto> CreateReviewAsync(string buyerId, ReviewDto reviewDto);
        Task<ReviewResponseDto> UpdateReviewAsync(int reviewId, string buyerId, ReviewDto reviewDto);
        Task<bool> DeleteReviewAsync(int reviewId, string buyerId);

        Task<IEnumerable<ReviewResponseDto>> GetArtworkReviewsAsync(Guid artworkId);
        Task<IEnumerable<ReviewResponseDto>> GetArtistReviewsAsync(int artistProfileId);
        Task<ReviewResponseDto?> GetReviewByIdAsync(int reviewId);

        Task<double> GetAverageRatingAsync(Guid? artworkId = null, int? artistProfileId = null);
        Task<int> GetReviewCountAsync(Guid? artworkId = null, int? artistProfileId = null);

        Task<bool> CanUserReviewAsync(string buyerId, Guid? artworkId, int? artistProfileId);
    }
}
