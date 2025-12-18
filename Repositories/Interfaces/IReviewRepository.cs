using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review?> GetReviewByIdAsync(int reviewId);
        Task<Review> CreateReviewAsync(Review review);
        Task<bool> UpdateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(int reviewId);

        Task<IEnumerable<Review>> GetReviewsByArtworkIdAsync(Guid artworkId);
        Task<IEnumerable<Review>> GetReviewsByArtistIdAsync(int artistProfileId);
        Task<IEnumerable<Review>> GetAllByArtistAsync(int artistProfileId);

        Task<bool> ReviewExistsAsync(int reviewId);
        Task<bool> IsReviewOwnerAsync(int reviewId, string userId);
    }
}
