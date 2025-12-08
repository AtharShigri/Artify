namespace Artify.Api.DTOs.Artist
{
    public class ArtServiceDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Duration { get; set; }   // e.g., "2 hours", "Per event"
        public string Category { get; set; }   // Singing, Dancing, Calligraphy, etc.
    }
}
