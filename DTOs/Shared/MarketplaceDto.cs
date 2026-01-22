namespace Artify.Api.DTOs.Shared
{
    public class ArtworkResponseDto
    {
        public Guid ArtworkId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public Guid ArtistProfileId { get; set; }
        public double Rating { get; set; }
        public int LikesCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsForSale { get; set; }
        public int Stock { get; set; }
    }

    public class ArtworkDetailDto : ArtworkResponseDto
    {
        public string ArtistBio { get; set; } = string.Empty;
        public string ArtistLocation { get; set; } = string.Empty;
        public string ArtistProfileImage { get; set; } = string.Empty;
        public List<string> ArtistSkills { get; set; } = new();
        public int ViewsCount { get; set; }
    }

    public class ArtistProfileDto
    {
        public Guid ArtistProfileId { get; set; }
        public Guid UserId { get; set; } = Guid.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = string.Empty;
        public double Rating { get; set; }
        public string PortfolioUrl { get; set; } = string.Empty;
        public List<string> Skills { get; set; } = new();
        public Dictionary<string, string> SocialLinks { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public int TotalArtworks { get; set; }
        public int TotalReviews { get; set; }

        // ✅ NEW: Added FeaturedArtworks property
        public List<ArtworkResponseDto> FeaturedArtworks { get; set; } = new();
    }

    public class SearchArtworksDto
    {
        public string Query { get; set; } = string.Empty;
        public string? Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; } = "newest";
    }
}