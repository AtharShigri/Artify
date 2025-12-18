using AutoMapper;
using Artify.Api.DTOs.Buyer;
using Artify.Api.DTOs.Shared;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Services.Implementations
{
    public class BuyerService : IBuyerService
    {
        private readonly IMapper _mapper;
        private readonly IBuyerRepository _buyerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IReviewRepository _reviewRepository;

        public BuyerService(
            IMapper mapper,
            IBuyerRepository buyerRepository,
            IOrderRepository orderRepository,
            IReviewRepository reviewRepository)
        {
            _mapper = mapper;
            _buyerRepository = buyerRepository;
            _orderRepository = orderRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<BuyerProfileResponseDto?> GetBuyerProfileAsync(string buyerId)
        {
            var user = await _buyerRepository.GetBuyerByIdAsync(buyerId);
            if (user == null) return null;

            var profileDto = _mapper.Map<BuyerProfileResponseDto>(user);

            // Get statistics
            var orders = await _orderRepository.GetOrdersByBuyerIdAsync(buyerId);
            profileDto.TotalOrders = orders.Count();

            // Get reviews made by this buyer
            profileDto.TotalReviews = 0; // We don't have a method to get reviews by buyer yet

            return profileDto;
        }

        public async Task<BuyerProfileResponseDto?> UpdateBuyerProfileAsync(string buyerId, BuyerUpdateDto updateDto)
        {
            var user = await _buyerRepository.GetBuyerByIdAsync(buyerId);
            if (user == null) return null;

            // Update only provided fields
            if (!string.IsNullOrWhiteSpace(updateDto.FullName))
                user.FullName = updateDto.FullName;

            if (!string.IsNullOrWhiteSpace(updateDto.PhoneNumber))
                user.PhoneNumber = updateDto.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(updateDto.ProfileImageUrl))
                user.ProfileImageUrl = updateDto.ProfileImageUrl;

            var updated = await _buyerRepository.UpdateBuyerAsync(user);
            if (!updated) return null;

            return await GetBuyerProfileAsync(buyerId);
        }

        public async Task<bool> DeleteBuyerAccountAsync(string buyerId)
        {
            return await _buyerRepository.DeleteBuyerAsync(buyerId);
        }

        public async Task<IEnumerable<ArtworkResponseDto>> GetFeaturedArtworksAsync()
        {
            var artworks = await _buyerRepository.GetFeaturedArtworksAsync();
            return _mapper.Map<IEnumerable<ArtworkResponseDto>>(artworks);
        }

        public async Task<IEnumerable<ArtworkResponseDto>> GetArtworksByCategoryAsync(Category category)
        {
            var artworks = await _buyerRepository.GetArtworksByCategoryAsync(category);
            return _mapper.Map<IEnumerable<ArtworkResponseDto>>(artworks);
        }

        /*public async Task<IEnumerable<ArtworkResponseDto>> SearchArtworksAsync(string query, Category? category = null)
        {
            var artworks = await _buyerRepository.SearchArtworksAsync(query);
            if (!string.IsNullOrEmpty(category))
            {
                artworks = artworks.Where(a => a.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }
            return _mapper.Map<IEnumerable<ArtworkResponseDto>>(artworks);
        }*/

        public async Task<ArtworkDetailDto?> GetArtworkDetailAsync(Guid artworkId)
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

        public async Task<ArtistProfileDto?> GetArtistProfileAsync(int artistProfileId)
        {
            var artistProfile = await _buyerRepository.GetArtistProfileByIdAsync(artistProfileId);
            if (artistProfile == null) return null;

            var dto = _mapper.Map<ArtistProfileDto>(artistProfile);

            // Set FeaturedArtworks
            if (artistProfile.Artworks != null && artistProfile.Artworks.Any())
            {
                dto.FeaturedArtworks = _mapper.Map<List<ArtworkResponseDto>>(
                    artistProfile.Artworks
                        .Where(a => a.IsForSale && a.Stock > 0) // Only available artworks
                        .OrderByDescending(a => a.LikesCount)   // Most popular first
                        .Take(6)                                // Top 6
                        .ToList());
            }

            // Get artist's rating
            var reviews = await _reviewRepository.GetReviewsByArtistIdAsync(artistProfileId);
            dto.Rating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;
            dto.TotalReviews = reviews.Count();
            dto.TotalArtworks = artistProfile.Artworks?.Count ?? 0;

            return dto;
        }

        public async Task<int> GetTotalOrdersAsync(string buyerId)
        {
            var orders = await _orderRepository.GetOrdersByBuyerIdAsync(buyerId);
            return orders.Count();
        }

        public async Task<int> GetTotalReviewsAsync(string buyerId)
        {
            // Note: We don't have a method to get reviews by buyer in IReviewRepository
            // This would need to be added to the repository interface
            return 0;
        }
    }
}