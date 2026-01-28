using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review?> GetReviewByIdAsync(Guid reviewId);
        Task<Review> CreateReviewAsync(Review review);
        Task<bool> UpdateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(Guid reviewId);

        Task<IEnumerable<Review>> GetReviewsByArtworkIdAsync(Guid artworkId);
        Task<IEnumerable<Review>> GetReviewsByArtistIdAsync(Guid artistProfileId);
        Task<IEnumerable<Review>> GetAllByArtistAsync(Guid artistProfileId);

        Task<bool> ReviewExistsAsync(Guid reviewId);
        Task<bool> IsReviewOwnerAsync(Guid reviewId, Guid userId);
    }
}
