using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Controllers.Artist
{
    [Route("api/artist/dashboard")]
    [ApiController]
    [Authorize(Roles = "Artist")]
    public class ArtistDashboardController : ControllerBase
    {
        private readonly IArtistDashboardService _dashboardService;

        public ArtistDashboardController(IArtistDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            return Ok(await _dashboardService.GetSummaryAsync(User));
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            return Ok(await _dashboardService.GetOrdersAsync(User));
        }

        [HttpGet("reviews")]
        public async Task<IActionResult> GetReviews()
        {
            return Ok(await _dashboardService.GetReviewsAsync(User));
        }
    }
}
