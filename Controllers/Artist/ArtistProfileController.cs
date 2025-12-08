using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Artify.Api.DTOs.Artist;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Controllers.Artist
{
    [Route("api/artist")]
    [ApiController]
    [Authorize(Roles = "Artist")]
    public class ArtistProfileController : ControllerBase
    {
        private readonly IArtistProfileService _artistService;

        public ArtistProfileController(IArtistProfileService artistService)
        {
            _artistService = artistService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ArtistRegisterDto dto)
        {
            return Ok(await _artistService.RegisterAsync(dto));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            return Ok(await _artistService.LoginAsync(dto));
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            return Ok(await _artistService.GetProfileAsync(User));
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ArtistUpdateDto dto)
        {
            return Ok(await _artistService.UpdateProfileAsync(User, dto));
        }

        [HttpPut("profile/image")]
        public async Task<IActionResult> UpdateProfileImage([FromForm] IFormFile file)
        {
            return Ok(await _artistService.UpdateProfileImageAsync(User, file));
        }

        [HttpDelete("profile")]
        public async Task<IActionResult> DeleteProfile()
        {
            return Ok(await _artistService.DeleteProfileAsync(User));
        }
    }
}
