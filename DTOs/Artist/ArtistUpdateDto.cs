using Artify.Api.Validation;

namespace Artify.Api.DTOs.Artist
{
    public class ArtistUpdateDto
    {
        public string FullName { get; set; }
        public string Bio { get; set; }
        [ArtCategory]
        public string Category { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string SocialLink { get; set; }
    }
}
