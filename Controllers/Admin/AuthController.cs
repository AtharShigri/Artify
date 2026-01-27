using Microsoft.AspNetCore.Mvc;
using Artify.Api.DTOs.Auth;
using Artify.Api.Services.Interfaces;
using Artify.Api.DTOs.Artist;

namespace Artify.Api.Controllers.Admin
{
    [Route("api/admin")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] DTOs.Auth.LoginDto dto)
        {
            try
            {
                var result = await _authService.LoginAdminAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
