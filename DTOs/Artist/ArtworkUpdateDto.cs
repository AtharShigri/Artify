using Artify.Api.Models;

namespace Artify.Api.DTOs.Artist
{
    public class ArtworkUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public Category Category { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
