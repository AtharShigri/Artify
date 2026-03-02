using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    public class Artwork
{
    [Key]
    public Guid ArtworkId { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ArtistProfileId { get; set; }

    [ForeignKey("ArtistProfileId")]
    public virtual ArtistProfile ArtistProfile { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    
    public string? ImageUrl { get; set; }
    public string? HashValue { get; set; } // For Plagiarism detection
    public string? Metadata { get; set; }
    
    public bool IsForSale { get; set; } = true;
    public bool IsApproved { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
    
    public int LikesCount { get; set; } = 0;
    public int ViewsCount { get; set; } = 0;
    public int Stock { get; set; } = 1;
    public string? Status { get; set; } // Draft, Published, Sold
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid? CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public virtual Category? CategoryEntity { get; set; }

    // Relationships
    public virtual ICollection<ArtworkTag> Tags { get; set; } = new List<ArtworkTag>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    // Plagiarism Tracking (Self-referencing relationships often need Fluent API config)
    public virtual ICollection<PlagiarismLog> PlagiarismLogsAsOriginal { get; set; } = new List<PlagiarismLog>();
    public virtual ICollection<PlagiarismLog> PlagiarismLogsAsSuspect { get; set; } = new List<PlagiarismLog>();
}
}
