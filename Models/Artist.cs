using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace Artify.Api.Models
{
    public class Artist : IdentityUser
    {

        [Key]
        public int Id { get; set; }

        // FK to ApplicationUser (Identity)
        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

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
        public string Email { get; internal set; }
    }
}
