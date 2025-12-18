using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    public class ArtistProfile
    {
        [Key]
        public Guid ArtistProfileId { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public string Bio { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public string ProfileImageUrl { get; set; }
        public float Rating { get; set; }
        public string PortfolioUrl { get; set; }

        public string SocialLinks { get; set; } // JSON or comma-separated URLs
        public string Skills { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Artwork> Artworks { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
