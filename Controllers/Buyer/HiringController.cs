using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Controllers.Buyer
{
    [Route("api/buyer/hire")]
    [ApiController]
    [Authorize(Roles = "Buyer")]
    public class HiringController : ControllerBase
    {
        private readonly IHiringService _hiringService;
        private readonly ILogger<HiringController> _logger;

        public HiringController(
            IHiringService hiringService,
            ILogger<HiringController> logger)
        {
            _hiringService = hiringService;
            _logger = logger;
        }

        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        /// <summary>
        /// Hire an artist
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(HiringResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> HireArtist([FromBody] HireArtistDto hireDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var hiringRequest = await _hiringService.CreateHiringRequestAsync(buyerId, hireDto);
                return Ok(hiringRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating hiring request");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get all hiring requests for current buyer
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HiringResponseDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetHiringRequests()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var requests = await _hiringService.GetBuyerHiringRequestsAsync(buyerId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting hiring requests");
                return StatusCode(500, new { message = "An error occurred while fetching hiring requests" });
            }
        }

        /// <summary>
        /// Get specific hiring request
        /// </summary>
        [HttpGet("{requestId}")]
        [ProducesResponseType(typeof(HiringResponseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetHiringRequest(Guid requestId)
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var request = await _hiringService.GetHiringRequestAsync(requestId, buyerId);
                if (request == null)
                    return NotFound(new { message = "Hiring request not found" });

                return Ok(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting hiring request");
                return StatusCode(500, new { message = "An error occurred while fetching hiring request" });
            }
        }

        /// <summary>
        /// Delete hiring request
        /// </summary>
        [HttpDelete("{requestId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteHiringRequest(Guid requestId)
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var result = await _hiringService.DeleteHiringRequestAsync(requestId, buyerId);
                if (!result)
                    return BadRequest(new { message = "Cannot delete this hiring request" });

                return Ok(new { message = "Hiring request deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting hiring request");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Initiate communication with artist for a hiring request
        /// </summary>
        [HttpPost("{requestId}/communicate")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> InitiateCommunication(Guid requestId)
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var message = await _hiringService.InitiateArtistCommunicationAsync(requestId);
                return Ok(new { message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initiating communication");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}