using Artify.Api.Data;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Artify.Api.Repositories.Implementations
{
    public class ReviewRepository : BaseRepository, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Review?> GetReviewByIdAsync(int reviewId)
        {
            return await _context.Reviews
                .Include(r => r.Reviewer)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }

        public async Task<IEnumerable<Review>> GetReviewsByArtworkIdAsync(int artworkId)
        {
            return await _context.Reviews
                .Where(r => r.ArtworkId == artworkId)
                .OrderByDescending(r => r.CreatedAt)
                .Include(r => r.Reviewer)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByArtistIdAsync(int artistProfileId)
        {
            return await _context.Reviews
                .Where(r => r.ArtistProfileId == artistProfileId)
                .OrderByDescending(r => r.CreatedAt)
                .Include(r => r.Reviewer)
                .ToListAsync();
        }

        public async Task<Review> CreateReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await SaveAsync();
            return review;
        }

        public async Task<bool> UpdateReviewAsync(Review review)
        {
            _context.Reviews.Update(review);
            return await SaveAsync();
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            var review = await GetReviewByIdAsync(reviewId);
            if (review == null)
                return false;

            _context.Reviews.Remove(review);
            return await SaveAsync();
        }

        public async Task<bool> ReviewExistsAsync(int reviewId)
        {
            return await _context.Reviews.AnyAsync(r => r.ReviewId == reviewId);
        }

        public async Task<bool> IsReviewOwnerAsync(int reviewId, string userId)
        {
            return await _context.Reviews
                .AnyAsync(r => r.ReviewId == reviewId && r.ReviewerId == userId);
        }
    }
}