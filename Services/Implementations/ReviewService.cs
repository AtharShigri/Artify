using AutoMapper;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Services.Implementations
{
    public class ReviewService : BaseService, IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(
            IMapper mapper,
            IBuyerRepository buyerRepository,
            IReviewRepository reviewRepository)
            : base(mapper, buyerRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<ReviewResponseDto> CreateReviewAsync(string buyerId, ReviewDto reviewDto)
        {
            // Validate that at least one of ArtworkId or ArtistProfileId is provided
            if (!reviewDto.ArtworkId.HasValue && !reviewDto.ArtistProfileId.HasValue)
                throw new Exception("Either ArtworkId or ArtistProfileId must be provided");

            // Check if user already reviewed
            if (!await CanUserReviewAsync(buyerId, reviewDto.ArtworkId, reviewDto.ArtistProfileId))
                throw new Exception("You have already reviewed this item/artist");

            var review = new Review
            {
                ReviewerId = buyerId,
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
                ArtworkId = reviewDto.ArtworkId,
                ArtistProfileId = reviewDto.ArtistProfileId,
                CreatedAt = DateTime.UtcNow
            };

            var createdReview = await _reviewRepository.CreateReviewAsync(review);
            return await MapReviewToDto(createdReview);
        }

        public async Task<ReviewResponseDto> UpdateReviewAsync(int reviewId, string buyerId, ReviewDto reviewDto)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null || review.ReviewerId != buyerId)
                throw new Exception("Review not found or unauthorized");

            review.Rating = reviewDto.Rating;
            review.Comment = reviewDto.Comment;
            review.CreatedAt = DateTime.UtcNow; // Update timestamp

            await _reviewRepository.UpdateReviewAsync(review);
            return await MapReviewToDto(review);
        }

        public async Task<bool> DeleteReviewAsync(int reviewId, string buyerId)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null || review.ReviewerId != buyerId)
                return false;

            return await _reviewRepository.DeleteReviewAsync(reviewId);
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetArtworkReviewsAsync(Guid artworkId)
        {
            var reviews = await _reviewRepository.GetReviewsByArtworkIdAsync(artworkId);
            var reviewDtos = new List<ReviewResponseDto>();

            foreach (var review in reviews)
            {
                reviewDtos.Add(await MapReviewToDto(review));
            }

            return reviewDtos;
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetArtistReviewsAsync(int artistProfileId)
        {
            var reviews = await _reviewRepository.GetReviewsByArtistIdAsync(artistProfileId);
            var reviewDtos = new List<ReviewResponseDto>();

            foreach (var review in reviews)
            {
                reviewDtos.Add(await MapReviewToDto(review));
            }

            return reviewDtos;
        }

        public async Task<ReviewResponseDto?> GetReviewByIdAsync(int reviewId)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null) return null;

            return await MapReviewToDto(review);
        }

        public async Task<double> GetAverageRatingAsync(Guid? artworkId = null, int? artistProfileId = null)
        {
            IEnumerable<Review> reviews;
            if (artworkId.HasValue)
            {
                reviews = await _reviewRepository.GetReviewsByArtworkIdAsync(artworkId.Value);
            }
            else if (artistProfileId.HasValue)
            {
                reviews = await _reviewRepository.GetReviewsByArtistIdAsync(artistProfileId.Value);
            }
            else
            {
                return 0;
            }

            if (!reviews.Any()) return 0;
            return reviews.Average(r => r.Rating);
        }

        public async Task<int> GetReviewCountAsync(Guid? artworkId = null, int? artistProfileId = null)
        {
            IEnumerable<Review> reviews;
            if (artworkId.HasValue)
            {
                reviews = await _reviewRepository.GetReviewsByArtworkIdAsync(artworkId.Value);
            }
            else if (artistProfileId.HasValue)
            {
                reviews = await _reviewRepository.GetReviewsByArtistIdAsync(artistProfileId.Value);
            }
            else
            {
                return 0;
            }

            return reviews.Count();
        }

        public async Task<bool> CanUserReviewAsync(string buyerId, Guid? artworkId, int? artistProfileId)
        {
            return !await HasUserReviewedAsync(buyerId, artworkId, artistProfileId);
        }

        private async Task<bool> HasUserReviewedAsync(string buyerId, Guid? artworkId, int? artistProfileId)
        {
            if (artworkId.HasValue)
            {
                var reviews = await _reviewRepository.GetReviewsByArtworkIdAsync(artworkId.Value);
                return reviews.Any(r => r.ReviewerId == buyerId);
            }

            if (artistProfileId.HasValue)
            {
                var reviews = await _reviewRepository.GetReviewsByArtistIdAsync(artistProfileId.Value);
                return reviews.Any(r => r.ReviewerId == buyerId);
            }

            return false;
        }


        private async Task<ReviewResponseDto> MapReviewToDto(Review review)
        {
            var dto = new ReviewResponseDto
            {
                ReviewId = review.ReviewId,
                ReviewerId = review.ReviewerId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                ArtworkId = review.ArtworkId,
                ArtistProfileId = review.ArtistProfileId
            };

            // Get reviewer details
            var reviewer = await _buyerRepository.GetBuyerByIdAsync(review.ReviewerId);
            if (reviewer != null)
            {
                dto.ReviewerName = reviewer.FullName;
                dto.ReviewerProfileImage = reviewer.ProfileImageUrl;
            }

            return dto;
        }
    }
}