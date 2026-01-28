// ========================= AdminUsersController.cs =========================
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Artify.Api.Services.Interfaces;
using Artify.Api.DTOs.Admin;

namespace Artify.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : ControllerBase
    {
        private readonly IAdminUserService _service;

        public AdminUsersController(IAdminUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _service.GetAllUsersAsync();
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var result = await _service.GetUserByIdAsync(userId);
            return Ok(result);
        }

        [HttpPut("status/{userId}")]
        public async Task<IActionResult> UpdateUserStatus(Guid userId, [FromBody] UpdateUserStatusDto dto)
        {
            var result = await _service.UpdateUserStatusAsync(userId, dto);
            return Ok(result);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> SoftDelete(Guid userId)
        {
            var result = await _service.SoftDeleteUserAsync(userId);
            return Ok(result);
        }
    }
}
