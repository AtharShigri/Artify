using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artify.Api.Models
{
    public class Artist : IdentityUser<Guid>
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [MaxLength(500)]
        public string? Bio { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(200)]
        public string? SocialLink { get; set; }

        [MaxLength(200)]
        public string? ProfileImageUrl { get; set; }

        public int TotalSales { get; set; } = 0;

        public ICollection<Artwork> Artworks { get; set; } = new List<Artwork>();
    }
}
