using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllByArtistAsync(int artistId);
        Task<Review> GetByIdAsync(int reviewId);
        Task AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(Review review);
    }
}
        // Review operations
        Task<Review?> GetReviewByIdAsync(int reviewId);
        Task<IEnumerable<Review>> GetReviewsByArtworkIdAsync(int artworkId);
        Task<IEnumerable<Review>> GetReviewsByArtistIdAsync(int artistProfileId);
        Task<Review> CreateReviewAsync(Review review);
        Task<bool> UpdateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(int reviewId);

        // Validation
        Task<bool> ReviewExistsAsync(int reviewId);
        Task<bool> IsReviewOwnerAsync(int reviewId, string userId);
    }
}
