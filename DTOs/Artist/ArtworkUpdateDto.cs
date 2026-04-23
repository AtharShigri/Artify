using Microsoft.AspNetCore.Http;

namespace Artify.Api.DTOs.Artist
{
    public class ArtworkUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Metadata { get; set; }
        public bool? IsAvailable { get; set; }
        public IFormFile? File { get; set; }
        public bool? ApplyWatermark { get; set; }
        public bool? RegisterFingerprint { get; set; }
        public string? CopyrightText { get; set; }
    }
}
