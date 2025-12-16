using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Artify.Api.Models
{
    public class Artist : IdentityUser<int>
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(500)]
        public string Bio { get; set; }

        [MaxLength(100)]
        public string Category { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(200)]
        public string SocialLink { get; set; }

        [MaxLength(200)]
        public string ProfileImageUrl { get; set; }
﻿// ========================= Artist.cs =========================
using System;
using System.Collections.Generic;

namespace Artify.Api.Models
{
    public class Artist
    {
        public string Id { get; set; }               // maps to ApplicationUser.Id
        public string FullName { get; set; }
        public int TotalSales { get; set; }

        public ICollection<Artwork>? Artworks { get; set; } = new List<Artwork>();
    }
}
