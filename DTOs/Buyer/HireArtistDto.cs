using System.ComponentModel.DataAnnotations;

namespace Artify.Api.DTOs.Buyer
{
    public class HireArtistDto
    {
        [Required] public int ArtistProfileId { get; set; }
        [Required][StringLength(200)] public string ProjectTitle { get; set; } = string.Empty;
        [Required][StringLength(1000)] public string ProjectDescription { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public DateTime Deadline { get; set; }
        public string ContactPreference { get; set; } = "Email";
    }

    public class HiringResponseDto
    {
        public Guid RequestId { get; set; }
        public string BuyerId { get; set; } = string.Empty;
        public string BuyerName { get; set; } = string.Empty;
        public int ArtistProfileId { get; set; }
        public string ArtistName { get; set; } = string.Empty;
        public string ProjectTitle { get; set; } = string.Empty;
        public string ProjectDescription { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; }
    }
}