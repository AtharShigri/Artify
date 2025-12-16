// ========================= Artist.cs =========================
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
