using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Artify.Api.DTOs.Shared;
using Artify.Api.Services.Interfaces;
using Artify.Api.Models;

namespace Artify.Api.Controllers.Shared
{
    [Route("api/marketplace")]
    [ApiController]
    [AllowAnonymous] // Public endpoints
    public class MarketplaceController : ControllerBase
    {
        private readonly IMarketplaceService _marketplaceService;
        private readonly ILogger<MarketplaceController> _logger;

        public MarketplaceController(
            IMarketplaceService marketplaceService,
            ILogger<MarketplaceController> logger)
        {
            _marketplaceService = marketplaceService;
            _logger = logger;
        }

        /// <summary>
        /// Get all artworks with pagination
        /// </summary>
        [HttpGet("artworks")]
        [ProducesResponseType(typeof(IEnumerable<ArtworkResponseDto>), 200)]
        public async Task<IActionResult> GetAllArtworks(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var artworks = await _marketplaceService.GetAllArtworksAsync(page, pageSize);
                return Ok(artworks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all artworks");
                return StatusCode(500, new { message = "An error occurred while fetching artworks" });
            }
        }

        /// <summary>
        /// Get artwork by ID
        /// </summary>
        [HttpGet("artworks/{id}")]
        [ProducesResponseType(typeof(ArtworkDetailDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetArtworkById(Guid id)
        {
            try
            {
                var artwork = await _marketplaceService.GetArtworkDetailsAsync(id);
                if (artwork == null)
                    return NotFound(new { message = "Artwork not found" });

                return Ok(artwork);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting artwork by ID");
                return StatusCode(500, new { message = "An error occurred while fetching artwork" });
            }
        }

        /// <summary>
        /// Get artworks by category
        /// </summary>
        [HttpGet("artworks/category/{category}")]
        [ProducesResponseType(typeof(IEnumerable<ArtworkResponseDto>), 200)]
        public async Task<IActionResult> GetArtworksByCategory(Category category)
        {
            try
            {
                var artworks = await _marketplaceService.GetArtworksByCategoryAsync(category);
                return Ok(artworks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting artworks by category");
                return StatusCode(500, new { message = "An error occurred while fetching artworks" });
            }
        }

        /// <summary>
        /// Search artworks with filters
        /// </summary>
        [HttpGet("artworks/search")]
        [ProducesResponseType(typeof(IEnumerable<ArtworkResponseDto>), 200)]
        public async Task<IActionResult> SearchArtworks([FromQuery] SearchArtworksDto searchDto)
        {
            try
            {
                var artworks = await _marketplaceService.SearchArtworksAsync(searchDto);
                return Ok(artworks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching artworks");
                return StatusCode(500, new { message = "An error occurred while searching artworks" });
            }
        }

        /// <summary>
        /// Get artist profile
        /// </summary>
        [HttpGet("artists/{artistId}")]
        [ProducesResponseType(typeof(ArtistProfileDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetArtistProfile(Guid artistId)
        {
            try
            {
                var artist = await _marketplaceService.GetArtistProfileAsync(artistId);
                if (artist == null)
                    return NotFound(new { message = "Artist not found" });

                return Ok(artist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting artist profile");
                return StatusCode(500, new { message = "An error occurred while fetching artist profile" });
            }
        }

        /// <summary>
        /// Get featured artists
        /// </summary>
        [HttpGet("artists/featured")]
        [ProducesResponseType(typeof(IEnumerable<ArtistProfileDto>), 200)]
        public async Task<IActionResult> GetFeaturedArtists()
        {
            try
            {
                var artists = await _marketplaceService.GetFeaturedArtistsAsync();
                return Ok(artists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting featured artists");
                return StatusCode(500, new { message = "An error occurred while fetching featured artists" });
            }
        }

        /// <summary>
        /// Get all available categories
        /// </summary>
        [HttpGet("categories")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _marketplaceService.GetArtworkCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories");
                return StatusCode(500, new { message = "An error occurred while fetching categories" });
            }
        }

        /// <summary>
        /// Get trending artworks
        /// </summary>
        [HttpGet("artworks/trending")]
        [ProducesResponseType(typeof(IEnumerable<ArtworkResponseDto>), 200)]
        public async Task<IActionResult> GetTrendingArtworks()
        {
            try
            {
                var artworks = await _marketplaceService.GetTrendingArtworksAsync();
                return Ok(artworks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trending artworks");
                return StatusCode(500, new { message = "An error occurred while fetching trending artworks" });
            }
        }
    }
}