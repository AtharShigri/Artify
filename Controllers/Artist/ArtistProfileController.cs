using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using artifi.Api.DTOs.Artist;
using artifi.Api.DTOs.Auth;
using artifi.Api.Services.Interfaces;
using artifi.Api.Services.Implementations;

namespace artifi.Api.Controllers.Artist
{
    [Route("api/artist")]
    [ApiController]
    [Authorize]
    public class ArtistProfileController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IArtistProfileService _artistProfileService;
        public ArtistProfileController(IAuthService authService, IArtistProfileService artistProfileService)
        {
            _authService = authService;
            _artistProfileService = artistProfileService;
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
