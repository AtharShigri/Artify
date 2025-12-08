using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Artify.Api.DTOs.Artist;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Controllers.Artist
{
    [Route("api/artist/artworks")]
    [ApiController]
    [Authorize(Roles = "Artist")]
    public class ArtworkController : ControllerBase
    {
        private readonly IArtworkService _artworkService;

        public ArtworkController(IArtworkService artworkService)
        {
            _artworkService = artworkService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _artworkService.GetAllAsync(User));
        }

        [HttpGet("{artworkId}")]
        public async Task<IActionResult> GetById(int artworkId)
        {
            return Ok(await _artworkService.GetByIdAsync(User, artworkId));
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] ArtworkUploadDto dto)
        {
            return Ok(await _artworkService.UploadAsync(User, dto));
        }

        [HttpPut("{artworkId}")]
        public async Task<IActionResult> Update(int artworkId, [FromBody] ArtworkUpdateDto dto)
        {
            return Ok(await _artworkService.UpdateAsync(User, artworkId, dto));
        }

        [HttpDelete("{artworkId}")]
        public async Task<IActionResult> Delete(int artworkId)
        {
            return Ok(await _artworkService.DeleteAsync(User, artworkId));
        }
    }
}
