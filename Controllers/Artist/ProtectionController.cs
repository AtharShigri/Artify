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

        [HttpPost("watermark")]
        public async Task<IActionResult> ApplyWatermark([FromForm] IFormFile file)
        {
            return Ok(await _protectionService.ApplyWatermarkAsync(User, file));
        }

        [HttpPost("metadata")]
        public async Task<IActionResult> EmbedMetadata([FromBody] MetadataDto dto)
        {
            return Ok(await _protectionService.EmbedMetadataAsync(User, dto));
        }

        [HttpPost("hash")]
        public async Task<IActionResult> GenerateHash([FromBody] HashDto dto)
        {
            return Ok(await _protectionService.GenerateHashAsync(User, dto));
        }

        [HttpPost("plagiarism-check")]
        public async Task<IActionResult> PlagiarismCheck([FromForm] IFormFile file)
        {
            return Ok(await _protectionService.CheckPlagiarismAsync(User, file));
        }
    }
}
