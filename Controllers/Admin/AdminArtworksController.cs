using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Artify.Api.Services.Interfaces;
using Artify.Api.DTOs.Admin;

namespace Artify.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/artworks")]
    [Authorize(Roles = "Admin")]
    public class AdminArtworksController : ControllerBase
    {
        private readonly IAdminArtworkService _service;

        public AdminArtworksController(IAdminArtworkService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetArtworks()
        {
            var result = await _service.GetAllArtworksAsync();
            return Ok(result);
        }

        [HttpGet("{artworkId}")]
        public async Task<IActionResult> GetArtwork(Guid artworkId)
        {
            var result = await _service.GetArtworkByIdAsync(artworkId);
            return Ok(result);
        }

        [HttpPut("approve/{artworkId}")]
        public async Task<IActionResult> ApproveArtwork(Guid artworkId)
        {
            var result = await _service.ApproveArtworkAsync(artworkId);
            return Ok(result);
        }

        [HttpPut("reject/{artworkId}")]
        public async Task<IActionResult> RejectArtwork(Guid artworkId, [FromBody] ArtworkModerationDto dto)
        {
            var result = await _service.RejectArtworkAsync(artworkId, dto);
            return Ok(result);
        }

        [HttpDelete("{artworkId}")]
        public async Task<IActionResult> RemoveArtwork(Guid artworkId)
        {
            var result = await _service.RemoveArtworkAsync(artworkId);
            return Ok(result);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingArtworks()
        {
            var result = await _service.GetPendingArtworksAsync();
            return Ok(result);
        }
    }
}
