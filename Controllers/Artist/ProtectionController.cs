using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Artify.Api.DTOs.Artist;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Controllers.Artist
{
    [Route("api/artist/protection")]
    [ApiController]
    [Authorize(Roles = "Artist")]
    public class ProtectionController : ControllerBase
    {
        private readonly IProtectionService _protectionService;

        public ProtectionController(IProtectionService protectionService)
        {
            _protectionService = protectionService;
        }

        /// <summary>Apply a copyright watermark to an uploaded image.</summary>
        [HttpPost("watermark")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ApplyWatermark([FromForm] WatermarkDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest(new { message = "No file uploaded." });

            var result = await _protectionService.ApplyWatermarkAsync(User, dto.File);
            return result.Success ? Ok(result) : BadRequest(new { message = "Failed to apply watermark." });
        }

        /// <summary>Embed copyright metadata for an existing artwork.</summary>
        [HttpPost("metadata")]
        public async Task<IActionResult> EmbedMetadata([FromBody] MetadataDto dto)
        {
            var result = await _protectionService.EmbedMetadataAsync(User, dto);
            return result.Success ? Ok(result) : BadRequest(new { message = "Artwork not found or access denied." });
        }

        /// <summary>Compute and register SHA-256 + perceptual hash for an artwork.</summary>
        [HttpPost("hash")]
        public async Task<IActionResult> GenerateHash([FromBody] HashDto dto)
        {
            var result = await _protectionService.GenerateHashAsync(User, dto);
            return result.Success ? Ok(result) : BadRequest(new { message = "Artwork not found, access denied, or image file missing." });
        }

        /// <summary>Check an uploaded image for plagiarism against all registered artworks.</summary>
        [HttpPost("plagiarism-check")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PlagiarismCheck([FromForm] PlagiarismCheckDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest(new { message = "No file uploaded." });

            var result = await _protectionService.CheckPlagiarismAsync(User, dto.File);
            return Ok(result);
        }

        /// <summary>Get the protection status (hashed? has metadata?) for a specific artwork.</summary>
        [HttpGet("status/{artworkId:guid}")]
        public async Task<IActionResult> GetProtectionStatus(Guid artworkId)
        {
            var result = await _protectionService.GetProtectionStatusAsync(User, artworkId);
            return result != null ? Ok(result) : NotFound(new { message = "Artwork not found or access denied." });
        }
    }
}
