using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Artify.Api.DTOs.Shared;
using Artify.Api.Services.Interfaces;
using Artify.Api.Models;
using System.Net.Mime;

namespace Artify.Api.Controllers.Shared
{
    [Route("api/marketplace")]
    [ApiController]
    [AllowAnonymous]
    [Produces(MediaTypeNames.Application.Json)]
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
        [ProducesResponseType(typeof(IEnumerable<ArtworkResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllArtworks(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                // Basic guard clauses for pagination
                page = page < 1 ? 1 : page;
                pageSize = pageSize > 100 ? 100 : pageSize;

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
        [HttpGet("artworks/{id:guid}")]
        [ProducesResponseType(typeof(ArtworkDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetArtworkById([FromRoute] Guid id)
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
                _logger.LogError(ex, "Error getting artwork by ID: {Id}", id);
                return StatusCode(500, new { message = "An error occurred while fetching artwork" });
            }
        }

        /// <summary>
        /// Get artworks by category
        /// </summary>
        [HttpGet("artworks/category/{category}")]
        [ProducesResponseType(typeof(IEnumerable<ArtworkResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetArtworksByCategory([FromRoute] Category category)
        {
            try
            {
                var artworks = await _marketplaceService.GetArtworksByCategoryAsync(category);
                return Ok(artworks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting artworks by category: {Category}", category);
                return StatusCode(500, new { message = "An error occurred while fetching artworks" });
            }
        }

        /// <summary>
        /// Search artworks with filters
        /// </summary>
        [HttpGet("artworks/search")]
        [ProducesResponseType(typeof(IEnumerable<ArtworkResponseDto>), StatusCodes.Status200OK)]
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
        [HttpGet("artists/{artistId:guid}")]
        [ProducesResponseType(typeof(ArtistProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetArtistProfile([FromRoute] Guid artistId)
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
                _logger.LogError(ex, "Error getting artist profile for: {ArtistId}", artistId);
                return StatusCode(500, new { message = "An error occurred while fetching artist profile" });
            }
        }

        /// <summary>
        /// Get all artists
        /// </summary>
        [HttpGet("artists")]
        [ProducesResponseType(typeof(IEnumerable<ArtistProfileDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllArtists(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                page = page < 1 ? 1 : page;
                pageSize = pageSize > 100 ? 100 : pageSize;

                var artists = await _marketplaceService.GetAllArtistsAsync(page, pageSize);
                return Ok(artists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all artists");
                return StatusCode(500, new { message = "An error occurred while fetching artists" });
            }
        }

        /// <summary>
        /// Get featured artists
        /// </summary>
        [HttpGet("artists/featured")]
        [ProducesResponseType(typeof(IEnumerable<ArtistProfileDto>), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(IEnumerable<ArtworkResponseDto>), StatusCodes.Status200OK)]
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