using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using artifi.Api.DTOs.Artist;
using artifi.Api.Services.Interfaces;

namespace artifi.Api.Controllers.Artist
{
    [Route("api/artist/artworks")]
    [ApiController]
    public class ArtworksController : ControllerBase
    {
        private readonly IArtworkService _artworkService;

        public ArtworksController(IArtworkService artworkService)
        {
            _artworkService = artworkService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _artworkService.GetAllAsync(User));
        }

        [HttpGet("{artworkId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid artworkId)
        {
            return Ok(await _artworkService.GetByIdAsync(User, artworkId));
        }

        [HttpPost("upload")]
        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> Upload([FromForm] ArtworkUploadDto dto)
        {
            return Ok(await _artworkService.UploadAsync(User, dto));
        }

        [HttpPut("{artworkId}")]
        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> Update(Guid artworkId, [FromForm] ArtworkUpdateDto dto)
        {
            return Ok(await _artworkService.UpdateAsync(User, artworkId, dto));
        }

        [HttpDelete("{artworkId}")]
        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> Delete(Guid artworkId)
        {
            return Ok(await _artworkService.DeleteAsync(User, artworkId));
        }
    }
}
