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
    public class BuyerHiringController : ControllerBase
    {
        private readonly IHiringService _hiringService;
        private readonly ILogger<BuyerHiringController> _logger;

        public BuyerHiringController(
            IHiringService hiringService,
            ILogger<BuyerHiringController> logger)
        {
            _hiringService = hiringService;
            _logger = logger;
        }

        private Guid? GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userId, out var guid) ? guid : null;
        }


        
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
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                var hiringRequest = await _hiringService.CreateHiringRequestAsync(buyerId.Value, hireDto);
                return Ok(hiringRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating hiring request");
                return BadRequest(new { message = ex.Message });
            }
        }

        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HiringResponseDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetHiringRequests()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                var requests = await _hiringService.GetBuyerHiringRequestsAsync(buyerId.Value);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting hiring requests");
                return StatusCode(500, new { message = "An error occurred while fetching hiring requests" });
            }
        }

        
        [HttpGet("{requestId}")]
        [ProducesResponseType(typeof(HiringResponseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetHiringRequest(Guid requestId)
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                var request = await _hiringService.GetHiringRequestAsync(requestId, buyerId.Value);
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
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                var result = await _hiringService.DeleteHiringRequestAsync(requestId, buyerId.Value);
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
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                var message = await _hiringService
                    .InitiateArtistCommunicationAsync(requestId, buyerId.Value);

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