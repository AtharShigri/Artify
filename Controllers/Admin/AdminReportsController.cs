// ========================= AdminReportsController.cs =========================
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Artify.Api.Services.Interfaces;
using Artify.Api.DTOs.Admin;

namespace Artify.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/reports")]
    [Authorize(Roles = "Admin")]
    public class AdminReportsController : ControllerBase
    {
        private readonly IAdminReportService _service;

        public AdminReportsController(IAdminReportService service)
        {
            _service = service;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _service.GetSummaryReportAsync();
            return Ok(result);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUserStats()
        {
            var result = await _service.GetUserReportAsync();
            return Ok(result);
        }

        [HttpGet("artworks")]
        public async Task<IActionResult> GetArtworkStats()
        {
            var result = await _service.GetArtworkReportAsync();
            return Ok(result);
        }

        [HttpGet("sales")]
        public async Task<IActionResult> GetSalesReport()
        {
            var result = await _service.GetSalesReportAsync();
            return Ok(result);
        }

        [HttpGet("registrations/monthly")]
        public async Task<IActionResult> GetMonthlyRegistrations()
        {
            var result = await _service.GetMonthlyRegistrationsAsync();
            return Ok(result);
        }

        [HttpGet("top-artists")]
        public async Task<IActionResult> GetTopArtists()
        {
            var result = await _service.GetTopArtistsAsync();
            return Ok(result);
        }

        [HttpGet("top-artworks")]
        public async Task<IActionResult> GetTopArtworks()
        {
            var result = await _service.GetTopArtworksAsync();
            return Ok(result);
        }

        [HttpGet("plagiarism-stats")]
        public async Task<IActionResult> GetPlagiarismStats()
        {
            var result = await _service.GetPlagiarismStatsAsync();
            return Ok(result);
        }
    }
}
