using AutoMapper;
using artifi.Api.Extensions;
using artifi.Api.DTOs.Shared;
using artifi.Api.Repositories.Interfaces;
using artifi.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using artifi.Api.Models;

namespace artifi.Api.Services.Implementations
{
    public class MarketplaceService : IMarketplaceService
    {
        private readonly IMapper _mapper;
        private readonly IBuyerRepository _buyerRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IArtworkRepository _artworkRepository;

        public MarketplaceService(
            IMapper mapper,
            IBuyerRepository buyerRepository,
            IReviewRepository reviewRepository,
            IArtworkRepository artworkRepository)
        {
            _mapper = mapper;
            _buyerRepository = buyerRepository;
            _reviewRepository = reviewRepository;
            _artworkRepository = artworkRepository;
        }

        public async Task<IEnumerable<ArtworkResponseDto>> GetAllArtworksAsync(int page = 1, int pageSize = 20)
        {
            var artworks = await _buyerRepository.GetArtworksByCategoryAsync(null, page, pageSize);
            return _mapper.Map<IEnumerable<ArtworkResponseDto>>(artworks);
        }

        public async Task<ArtworkDetailDto?> GetArtworkDetailsAsync(Guid artworkId)
        {
            var artwork = await _buyerRepository.GetArtworkByIdAsync(artworkId);
            if (artwork == null) return null;

            // Increment view count
            await _buyerRepository.IncrementArtworkViewsAsync(artworkId);

            var dto = _mapper.Map<ArtworkDetailDto>(artwork);

            // Get artist details
            if (artwork.ArtistProfile != null)
            {
                var reviews = await _reviewRepository.GetReviewsByArtistIdAsync(artwork.ArtistProfileId);
                dto.ArtistRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;
                dto.ArtistReviewCount = reviews.Count();
            }

            return dto;
        }

        public async Task<IEnumerable<ArtworkResponseDto>> GetArtworksByCategoryAsync(Category category)
        {
            var artworks = await _buyerRepository.GetArtworksByCategoryAsync(category);
            return _mapper.Map<IEnumerable<ArtworkResponseDto>>(artworks);
        }

        public async Task<IEnumerable<ArtworkResponseDto>> SearchArtworksAsync(SearchArtworksDto searchDto)
        {
            var artworks = await _buyerRepository.SearchArtworksAsync(
                searchDto.Query,
                searchDto.MinPrice,
                searchDto.MaxPrice,
                searchDto.SortBy ?? "newest");

            return _mapper.Map<IEnumerable<ArtworkResponseDto>>(artworks);
        }

        public async Task<ArtistProfileDto?> GetArtistProfileAsync(Guid artistId)
        {
            var artist = await _buyerRepository.GetArtistProfileByIdAsync(artistId);
            if (artist == null) return null;

            var dto = _mapper.Map<ArtistProfileDto>(artist);

            // Get reviews and rating
            var reviews = await _reviewRepository.GetReviewsByArtistIdAsync(artistId);
            dto.Rating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;
            dto.TotalReviews = reviews.Count();

            // Get artworks count
            var artworks = await _artworkRepository.GetAllByArtistAsync(artistId);
            dto.TotalArtworks = artworks.Count();
            dto.FeaturedArtworks = _mapper.Map<List<ArtworkResponseDto>>(artworks.Take(4));

            return dto;
        }

        public async Task<IEnumerable<ArtistProfileDto>> GetFeaturedArtistsAsync(int count = 10)
        {
            var artists = await _buyerRepository.GetFeaturedArtistsAsync(count);
            return await MapArtistsToDtos(artists);
        }

        public async Task<IEnumerable<ArtistProfileDto>> GetAllArtistsAsync(int page = 1, int pageSize = 20)
        {
            var artists = await _buyerRepository.GetAllArtistsAsync(page, pageSize);
            return await MapArtistsToDtos(artists);
        }

        private async Task<IEnumerable<ArtistProfileDto>> MapArtistsToDtos(IEnumerable<ArtistProfile> artists)
        {
            var artistDtos = new List<ArtistProfileDto>();
            var artistIds = artists.Select(a => a.ArtistProfileId).ToList();

            // Batch fetch all reviews for these artists
            var allReviews = await _reviewRepository.GetReviewsByArtistIdsAsync(artistIds);
            var reviewsByArtist = allReviews.GroupBy(r => r.ArtistProfileId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Batch fetch all artworks for these artists (to avoid N+1 count loading)
            var allArtworks = await _artworkRepository.GetArtworksByArtistIdsAsync(artistIds);
            var artworksByArtist = allArtworks.GroupBy(a => a.ArtistProfileId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var artist in artists)
            {
                var dto = _mapper.Map<ArtistProfileDto>(artist);

                // Get rating from our batched dictionary
                if (reviewsByArtist.TryGetValue(artist.ArtistProfileId, out var reviews))
                {
                    dto.Rating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;
                    dto.TotalReviews = reviews.Count();
                }
                else
                {
                    dto.Rating = 0;
                    dto.TotalReviews = 0;
                }

                // Get artworks count from our batched dictionary
                if (artworksByArtist.TryGetValue(artist.ArtistProfileId, out var artworks))
                {
                    dto.TotalArtworks = artworks.Count;
                    dto.FeaturedArtworks = _mapper.Map<List<ArtworkResponseDto>>(artworks.Take(4));
                }
                else
                {
                    dto.TotalArtworks = 0;
                    dto.FeaturedArtworks = new List<ArtworkResponseDto>();
                }

                artistDtos.Add(dto);
            }

            return artistDtos;
        }

        public async Task<IEnumerable<string>> GetArtworkCategoriesAsync()
        {
            var artworks = await _buyerRepository.GetArtworksByCategoryAsync(null, 1, 1000);

            return artworks
                .Select(a => a.CategoryEntity)
                .Distinct()
                .Where(c => !c.IsNullOrEmpty())
                .Select(c => c.Name)
                .ToList();
        }


        public async Task<IEnumerable<ArtworkResponseDto>> GetTrendingArtworksAsync()
        {
            // Trending = Most viewed + most liked in last 30 days
            var artworks = await _buyerRepository.GetFeaturedArtworksAsync(20);
            return _mapper.Map<IEnumerable<ArtworkResponseDto>>(artworks);
        }
    }
}