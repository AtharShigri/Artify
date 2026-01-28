using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    public class Artwork
    {
        [Key]
        public Guid ArtworkId { get; set; }

        [Required]
        public Guid ArtistProfileId { get; set; }

        [ForeignKey("ArtistProfileId")]
        public ArtistProfile ArtistProfile { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? HashValue { get; set; }
        public string? Metadata { get; set; }
        public bool IsForSale { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int LikesCount { get; set; }
        public int ViewsCount { get; set; }
        public int Stock { get; set; }
        public string? Status { get; set; } // Draft, Published, Sold, etc.

        public bool IsDeleted { get; set; }
        public bool IsApproved { get; set; }
        public Guid? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? CategoryEntity { get; set; }

        // New: Tags
        public ICollection<ArtworkTag> Tags { get; set; } = new List<ArtworkTag>();

        // New: Plagiarism logs
        public ICollection<PlagiarismLog> PlagiarismLogsAsOriginal { get; set; } = new List<PlagiarismLog>();
        public ICollection<PlagiarismLog> PlagiarismLogsAsSuspect { get; set; } = new List<PlagiarismLog>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
