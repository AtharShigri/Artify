using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    public class Review
    {
        [Key]
        public Guid ReviewId { get; set; }
        public Guid OrderId { get; set; }

        public Guid ReviewerId { get; set; }
        [ForeignKey("ReviewerId")]
        public ApplicationUser Reviewer { get; set; }

        public Guid? ArtistProfileId { get; set; }
        [ForeignKey("ArtistProfileId")]
        public ArtistProfile ArtistProfile { get; set; }

        public Guid? ArtworkId { get; set; }
        [ForeignKey("ArtworkId")]
        public Artwork Artwork { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
