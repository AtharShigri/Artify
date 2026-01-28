using System.ComponentModel.DataAnnotations;

namespace Artify.Api.DTOs.Buyer
{
    public class ReviewDto
    {
        [Required][Range(1, 5)] public int Rating { get; set; }
        [Required][StringLength(500)] public string Comment { get; set; } = string.Empty;
        public Guid? ArtworkId { get; set; }
        public Guid? ArtistProfileId { get; set; }
    }

    public class ReviewResponseDto
    {
        public Guid ReviewId { get; set; }
        public Guid ReviewerId { get; set; } = Guid.Empty;
        public string ReviewerName { get; set; } = string.Empty;
        public string ReviewerProfileImage { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid? ArtworkId { get; set; }
        public Guid? ArtistProfileId { get; set; }
    }
}