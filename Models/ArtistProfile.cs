using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artify.Api.Models
{
    public class ArtistProfile
{
    [Key]
    public Guid ArtistProfileId { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; } = null!;

    public string? Bio { get; set; }
    public string? Category { get; set; } // General category name
    public string? Location { get; set; }
    public string? ProfileImageUrl { get; set; }
    public float Rating { get; set; } = 0;
    public string? PortfolioUrl { get; set; }
    
    public string? SocialLinks { get; set; } 
    public string? Skills { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual ICollection<Artwork> Artworks { get; set; } = new List<Artwork>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
}
