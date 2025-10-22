using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    public class Artwork
    {
        [Key]
        public int ArtworkId { get; set; }

        [Required]
        public int ArtistProfileId { get; set; }

        [ForeignKey("ArtistProfileId")]
        public ArtistProfile ArtistProfile { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string HashValue { get; set; }
        public string Metadata { get; set; }
        public bool IsForSale { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int LikesCount { get; set; }
        public int ViewsCount { get; set; }
        public int Stock { get; set; }
        public string Status { get; set; } // Draft, Published, Sold, etc.
        public int? CategoryId { get; set; } // Future category FK


        public ICollection<Review> Reviews { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
