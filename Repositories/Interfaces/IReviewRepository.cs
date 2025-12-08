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
