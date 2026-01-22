using Artify.Api.DTOs.Buyer;

namespace Artify.Api.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDto> CreateReviewAsync(Guid buyerId, ReviewDto reviewDto);
        Task<ReviewResponseDto> UpdateReviewAsync(Guid reviewId, Guid buyerId, ReviewDto reviewDto);
        Task<bool> DeleteReviewAsync(Guid reviewId, Guid buyerId);

        Task<IEnumerable<ReviewResponseDto>> GetArtworkReviewsAsync(Guid artworkId);
        Task<IEnumerable<ReviewResponseDto>> GetArtistReviewsAsync(Guid artistProfileId);
        Task<ReviewResponseDto?> GetReviewByIdAsync(Guid reviewId);

        Task<double> GetAverageRatingAsync(Guid? artworkId = null, Guid? artistProfileId = null);
        Task<int> GetReviewCountAsync(Guid? artworkId = null, Guid? artistProfileId = null);

        Task<bool> CanUserReviewAsync(Guid buyerId, Guid? artworkId, Guid? artistProfileId);
    }
}
