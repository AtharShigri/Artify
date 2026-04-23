using Artify.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Artify.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/dashboard")]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _service;

        public AdminDashboardController(IAdminDashboardService service)
        {
            _service = service;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _service.GetDashboardStatsAsync();
            return Ok(stats);
        }

        [HttpGet("recent-users")]
        public async Task<IActionResult> GetRecentUsers([FromQuery] int count = 5)
        {
            var users = await _service.GetRecentUsersAsync(count);
            return Ok(users);
        }

        [HttpGet("recent-plagiarism-reports")]
        public async Task<IActionResult> GetRecentPlagiarismReports([FromQuery] int count = 5)
        {
            var reports = await _service.GetRecentPlagiarismReportsAsync(count);
            return Ok(reports);
        }
    }
}
