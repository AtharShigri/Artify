using Microsoft.AspNetCore.Mvc;
using Artify.Api.DTOs.Auth;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Controllers.Auth
{
    [Route("api/auth")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly IAuthService _authService;

        public PasswordController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            await _authService.ForgotPasswordAsync(dto.Email);
            return Ok(new { message = "Password reset email sent" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            await _authService.ResetPasswordAsync(dto);
            return Ok(new { message = "Password reset successful" });
        }
    }
}
