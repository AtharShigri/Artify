namespace Artify.Api.DTOs.Artist
{
    public class MetadataDto
    {
        public Guid ArtworkId { get; set; }
        public string ArtistName { get; set; }
        public string CopyrightText { get; set; }
        public string Description { get; set; }
    }
}
