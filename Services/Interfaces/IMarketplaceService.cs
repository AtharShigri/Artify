using Artify.Api.DTOs.Shared;

namespace Artify.Api.Services.Interfaces
{
    public interface IMarketplaceService
    {
        // Marketplace Operations (Public endpoints - no authentication required)
        Task<IEnumerable<ArtworkResponseDto>> GetAllArtworksAsync(int page = 1, int pageSize = 20);
        Task<ArtworkDetailDto?> GetArtworkDetailsAsync(int artworkId);
        Task<IEnumerable<ArtworkResponseDto>> GetArtworksByCategoryAsync(string category);
        Task<IEnumerable<ArtworkResponseDto>> SearchArtworksAsync(SearchArtworksDto searchDto);
        Task<ArtistProfileDto?> GetArtistProfileAsync(int artistProfileId);
        Task<IEnumerable<ArtistProfileDto>> GetFeaturedArtistsAsync(int count = 10);

        // Filters and Sorting
        Task<IEnumerable<string>> GetArtworkCategoriesAsync();
        Task<IEnumerable<ArtworkResponseDto>> GetTrendingArtworksAsync();
    }
}