// ========================= PlagiarismLogController.cs =========================
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using artifi.Api.Services.Interfaces;
using artifi.Api.DTOs.Admin;

namespace artifi.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/plagiarism")]
    [Authorize(Roles = "Admin")]
    public class PlagiarismLogController : ControllerBase
    {
        private readonly IPlagiarismService _service;

        public PlagiarismLogController(IPlagiarismService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            var result = await _service.GetPlagiarismLogsAsync();
            return Ok(result);
        }

        [HttpGet("{logId}")]
        public async Task<IActionResult> GetLog(Guid logId)
        {
            var result = await _service.GetPlagiarismLogByIdAsync(logId);
            return Ok(result);
        }

        [HttpPut("review/{logId}")]
        public async Task<IActionResult> MarkReviewed(Guid logId, [FromBody] PlagiarismReviewDto dto)
        {
            var result = await _service.MarkReviewedAsync(logId, dto);
            return Ok(result);
        }

        [HttpPut("action/{logId}")]
        public async Task<IActionResult> TakeAction(Guid logId, [FromBody] PlagiarismReviewDto dto)
        {
            var result = await _service.TakeActionAsync(logId, dto);
            return Ok(result);
        }
    }
}
