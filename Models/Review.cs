using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public string ReviewerId { get; set; }
        [ForeignKey("ReviewerId")]
        public ApplicationUser Reviewer { get; set; }

        public int? ArtistProfileId { get; set; }
        [ForeignKey("ArtistProfileId")]
        public ArtistProfile ArtistProfile { get; set; }

        public int? ArtworkId { get; set; }
        [ForeignKey("ArtworkId")]
        public Artwork Artwork { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
