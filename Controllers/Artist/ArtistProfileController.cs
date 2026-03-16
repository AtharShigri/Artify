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
        private readonly IAuthService _authService;
        private readonly IArtistProfileService _artistProfileService;
        public ArtistProfileController(IAuthService authService, IArtistProfileService artistProfileService)
        {
            _authService = authService;
            _artistProfileService = artistProfileService;
        }

        [AllowAnonymous]
[HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegisterDto dto)
{
    try 
    {
        var result = await _authService.RegisterArtistAsync(dto);
        return Ok(result); 
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}

        [AllowAnonymous]
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDto dto)
{
    try 
    {
        var result = await _authService.LoginArtistAsync(dto);
        return Ok(result); 
    }
    catch (Exception ex)
    {
        return Unauthorized(new { message = ex.Message });
    }
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
