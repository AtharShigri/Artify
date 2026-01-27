using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Artify.Api.DTOs.Artist;
using Artify.Api.DTOs.Auth;
using Artify.Api.Services.Interfaces;
using Artify.Api.Services.Implementations;

namespace Artify.Api.Controllers.Artist
{
    [Route("api/artist")]
    [ApiController]
    [Authorize(Roles = "Artist")]
    public class ArtistProfileController : ControllerBase
    {
        private readonly IArtistProfileService _artistProfileService;

        public ArtistProfileController(IArtistProfileService artistProfileService)
        {
            _artistProfileService = artistProfileService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ArtistRegisterDto dto)
        {
            var result = await _artistProfileService.RegisterAsync(dto);
            // Since result is an object, we serialize it to check properties (or better, change service return type).
            // For now, assuming result structure based on inspection:
            // return new { Success = true/false, ... }
            
            // Using dynamic to access properties of anonymous object
            dynamic dynamicResult = result;
            if (dynamicResult.Success == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _artistProfileService.LoginAsync(dto);
            dynamic dynamicResult = result;
            if (dynamicResult.Success == false)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            return Ok(await _artistProfileService.GetProfileAsync(User));
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ArtistUpdateDto dto)
        {
            return Ok(await _artistProfileService.UpdateProfileAsync(User, dto));
        }

        [HttpPut("profile-image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProfileImage(
    [FromForm] UpdateProfileImageDto dto)
        {
            var result = await _artistProfileService.UpdateProfileImageAsync(User, dto.Image);
            return Ok(result);
        }

        [HttpDelete("profile")]
        public async Task<IActionResult> DeleteProfile()
        {
            return Ok(await _artistProfileService.DeleteProfileAsync(User));
        }
    }
}
