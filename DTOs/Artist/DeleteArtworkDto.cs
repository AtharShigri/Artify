namespace Artify.Api.DTOs.Artist
{
    public class DeleteArtworkDto
    {
        public Guid ArtworkId { get; set; }
        public string Reason { get; set; }
    }
}
