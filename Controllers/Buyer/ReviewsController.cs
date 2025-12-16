using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Controllers.Buyer
{
    [Route("api/buyer/reviews")]
    [ApiController]
    [Authorize(Roles = "Buyer")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(
            IReviewService reviewService,
            ILogger<ReviewsController> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }

        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        /// <summary>
        /// Create a new review
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ReviewResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreateReview([FromBody] ReviewDto reviewDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var review = await _reviewService.CreateReviewAsync(buyerId, reviewDto);
                return Ok(review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating review");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update a review
        /// </summary>
        [HttpPut("{reviewId}")]
        [ProducesResponseType(typeof(ReviewResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] ReviewDto reviewDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var updatedReview = await _reviewService.UpdateReviewAsync(reviewId, buyerId, reviewDto);
                return Ok(updatedReview);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating review");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a review
        /// </summary>
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var result = await _reviewService.DeleteReviewAsync(reviewId, buyerId);
                if (!result)
                    return NotFound(new { message = "Review not found" });

                return Ok(new { message = "Review deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review");
                return StatusCode(500, new { message = "An error occurred while deleting review" });
            }
        }

        /// <summary>
        /// Get reviews for an artwork
        /// </summary>
        [HttpGet("artwork/{artworkId}")]
        [ProducesResponseType(typeof(IEnumerable<ReviewResponseDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetArtworkReviews(int artworkId)
        {
            try
            {
                var reviews = await _reviewService.GetArtworkReviewsAsync(artworkId);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting artwork reviews");
                return StatusCode(500, new { message = "An error occurred while fetching reviews" });
            }
        }

        /// <summary>
        /// Get reviews for an artist
        /// </summary>
        [HttpGet("artist/{artistProfileId}")]
        [ProducesResponseType(typeof(IEnumerable<ReviewResponseDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetArtistReviews(int artistProfileId)
        {
            try
            {
                var reviews = await _reviewService.GetArtistReviewsAsync(artistProfileId);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting artist reviews");
                return StatusCode(500, new { message = "An error occurred while fetching reviews" });
            }
        }

        /// <summary>
        /// Get average rating for artwork or artist
        /// </summary>
        [HttpGet("average-rating")]
        [ProducesResponseType(typeof(double), 200)]
        public async Task<IActionResult> GetAverageRating(
            [FromQuery] int? artworkId = null,
            [FromQuery] int? artistProfileId = null)
        {
            try
            {
                var average = await _reviewService.GetAverageRatingAsync(artworkId, artistProfileId);
                return Ok(new { averageRating = average });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting average rating");
                return StatusCode(500, new { message = "An error occurred while calculating average rating" });
            }
        }
    }
}