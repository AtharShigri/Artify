using AutoMapper;
using Artify.Api.Extensions;
using Artify.Api.DTOs.Shared;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Artify.Api.Models;

namespace Artify.Api.Services.Implementations
{
    public class MarketplaceService : IMarketplaceService
    {
        private readonly IMapper _mapper;
        private readonly IBuyerRepository _buyerRepository;
        private readonly IReviewRepository _reviewRepository;

        public MarketplaceService(
            IMapper mapper,
            IBuyerRepository buyerRepository,
            IReviewRepository reviewRepository)
        {
            _mapper = mapper;
            _buyerRepository = buyerRepository;
            _reviewRepository = reviewRepository;
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
                dto.ArtistBio = artwork.ArtistProfile.Bio ?? "";
                dto.ArtistLocation = artwork.ArtistProfile.Location ?? "";
                dto.ArtistProfileImage = artwork.ArtistProfile.ProfileImageUrl ?? "";
                dto.ArtistSkills = !string.IsNullOrEmpty(artwork.ArtistProfile.Skills)
                    ? artwork.ArtistProfile.Skills.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim()).ToList()
                    : new List<string>();
            }

            // Get average rating
            var reviews = await _reviewRepository.GetReviewsByArtworkIdAsync(artworkId);
            dto.Rating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

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

        public async Task<ArtistProfileDto?> GetArtistProfileAsync(Guid artistProfileId)
        {
            var artistProfile = await _buyerRepository.GetArtistProfileByIdAsync(artistProfileId);
            if (artistProfile == null) return null;

            var dto = _mapper.Map<ArtistProfileDto>(artistProfile);

            // Set FeaturedArtworks
            if (artistProfile.Artworks != null && artistProfile.Artworks.Any())
            {
                dto.FeaturedArtworks = _mapper.Map<List<ArtworkResponseDto>>(
                    artistProfile.Artworks
                        .Where(a => a.IsForSale && a.Stock > 0)
                        .OrderByDescending(a => a.LikesCount)
                        .Take(6)
                        .ToList());
            }

            // Get artist's rating
            var reviews = await _reviewRepository.GetReviewsByArtistIdAsync(artistProfileId);
            dto.Rating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;
            dto.TotalReviews = reviews.Count();
            dto.TotalArtworks = artistProfile.Artworks?.Count ?? 0;

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

            foreach (var artist in artists)
            {
                var dto = _mapper.Map<ArtistProfileDto>(artist);

                // Get rating
                var reviews = await _reviewRepository.GetReviewsByArtistIdAsync(artist.ArtistProfileId);
                dto.Rating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;
                dto.TotalReviews = reviews.Count();

                // Get artworks count
                dto.TotalArtworks = artist.Artworks?.Count ?? 0;

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