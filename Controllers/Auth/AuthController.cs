using Microsoft.AspNetCore.Mvc;
using Artify.Api.DTOs.Auth;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Controllers.Auth
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // ---------------- REGISTRATION ----------------

        [HttpPost("register/buyer")]
        public async Task<IActionResult> RegisterBuyer([FromBody] RegisterDto dto)
        {
            try
            {
                // Forces the "Buyer" role while allowing the user to choose Individual or Agency type
                var result = await _authService.RegisterUserAsync(dto, "Buyer");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("register/artist")]
        public async Task<IActionResult> RegisterArtist([FromBody] RegisterDto dto)
        {
            try
            {
                // Forces the "Artist" role and initializes the ArtistProfile
                var result = await _authService.RegisterUserAsync(dto, "Artist");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ---------------- LOGIN ----------------

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                // Unified login: Identity handles the role/type check via the service
                var result = await _authService.LoginAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }


    }
}