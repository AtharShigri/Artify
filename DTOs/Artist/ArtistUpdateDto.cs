using artifi.Api.Validation;

namespace artifi.Api.DTOs.Artist
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
        public int? TeamSize { get; set; }
        public string? MemberNames { get; set; }
    }
}
