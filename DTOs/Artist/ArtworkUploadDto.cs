using Artify.Api.Models;
using Microsoft.AspNetCore.Http;

namespace Artify.Api.DTOs.Artist
{
    public class ArtworkUploadDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Metadata { get; set; }
        public IFormFile File { get; set; }   
    }
}
