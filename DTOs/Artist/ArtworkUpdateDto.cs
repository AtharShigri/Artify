namespace Artify.Api.DTOs.Artist
{
    public class ArtworkUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public string Category { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
