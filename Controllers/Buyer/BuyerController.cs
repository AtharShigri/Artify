using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AutoMapper;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Controllers.Buyer
{
    [Route("api/buyer")]
    [ApiController]
    [Authorize(Roles = "Buyer")]
    public class BuyerController : ControllerBase
    {
        private readonly IBuyerService _buyerService;
        private readonly IMapper _mapper;
        private readonly ILogger<BuyerController> _logger;

        public BuyerController(
            IBuyerService buyerService,
            IMapper mapper,
            ILogger<BuyerController> logger)
        {
            _buyerService = buyerService;
            _mapper = mapper;
            _logger = logger;
        }

        // Helper method to get current user ID
        private Guid? GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userId, out var guid) ? guid : null;
        }


        /// <summary>
        /// Get current buyer's profile
        /// </summary>
        /// <returns>Buyer profile information</returns>
        [HttpGet("profile")]
        [ProducesResponseType(typeof(BuyerProfileResponseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                var profile = await _buyerService.GetBuyerProfileAsync(buyerId.Value);
                if (profile == null)
                    return NotFound(new { message = "Buyer profile not found" });

                return Ok(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting buyer profile");
                return StatusCode(500, new { message = "An error occurred while fetching profile" });
            }
        }

        /// <summary>
        /// Update buyer profile
        /// </summary>
        /// <param name="updateDto">Profile update data</param>
        /// <returns>Updated profile</returns>
        [HttpPut("profile")]
        [ProducesResponseType(typeof(BuyerProfileResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateProfile([FromBody] BuyerUpdateDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var buyerId = GetCurrentUserId();
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                var updatedProfile = await _buyerService.UpdateBuyerProfileAsync(buyerId.Value, updateDto);
                if (updatedProfile == null)
                    return NotFound(new { message = "Buyer not found" });

                return Ok(updatedProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating buyer profile");
                return StatusCode(500, new { message = "An error occurred while updating profile" });
            }
        }

        /// <summary>
        /// Delete buyer account
        /// </summary>
        /// <returns>Success message</returns>
        [HttpDelete("profile")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAccount()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                var result = await _buyerService.DeleteBuyerAccountAsync(buyerId.Value);
                if (!result)
                    return NotFound(new { message = "Buyer not found" });

                return Ok(new { message = "Account deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting buyer account");
                return StatusCode(500, new { message = "An error occurred while deleting account" });
            }
        }

        /// <summary>
        /// Get buyer's total orders count
        /// </summary>
        /// <returns>Order count</returns>
        [HttpGet("orders/count")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetOrdersCount()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                var count = await _buyerService.GetTotalOrdersAsync(buyerId.Value);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders count");
                return StatusCode(500, new { message = "An error occurred while fetching orders count" });
            }
        }

        /// <summary>
        /// Get buyer's total reviews count
        /// </summary>
        /// <returns>Reviews count</returns>
        [HttpGet("reviews/count")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetReviewsCount()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                var count = await _buyerService.GetTotalReviewsAsync(buyerId.Value);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reviews count");
                return StatusCode(500, new { message = "An error occurred while fetching reviews count" });
            }
        }
    }
}