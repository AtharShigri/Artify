using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Artify.Api.Services.Interfaces;
using System.Security.Claims;

namespace Artify.Api.Controllers.Artist
{
    [Route("api/artist/hire")]
    [ApiController]
    [Authorize(Roles = "Artist")]
    public class ArtistHiringController : ControllerBase
    {
        private readonly IHiringService _hiringService;

        public ArtistHiringController(IHiringService hiringService)
        {
            _hiringService = hiringService;
        }

        // Helper to get the Artist ID from the JWT Token
        private Guid GetArtistId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var guid))
            {
                throw new UnauthorizedAccessException("Invalid user identity.");
            }
            return guid;
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetRequests()
        {
            try
            {
                var artistId = GetArtistId();
                var result = await _hiringService.GetArtistRequestsAsync(artistId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPut("{requestId}/accept")]
        public async Task<IActionResult> Accept(Guid requestId)
        {
            try
            {
                var artistId = GetArtistId();
                await _hiringService.AcceptRequestAsync(artistId, requestId);
                return Ok(new { message = "Hiring request accepted" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{requestId}/reject")]
        public async Task<IActionResult> Reject(Guid requestId)
        {
            try
            {
                var artistId = GetArtistId();
                await _hiringService.RejectRequestAsync(artistId, requestId);
                return Ok(new { message = "Hiring request rejected" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}