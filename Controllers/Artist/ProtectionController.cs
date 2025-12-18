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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ApplyWatermark([FromForm] WatermarkDto dto)
        {
            return Ok(await _protectionService.ApplyWatermarkAsync(User, dto.File));
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PlagiarismCheck([FromForm] PlagiarismCheckDto dto)
        {
            return Ok(await _protectionService.CheckPlagiarismAsync(User, dto.File));
        }
    }
}
