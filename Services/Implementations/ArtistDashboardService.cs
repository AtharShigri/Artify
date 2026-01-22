using System.Security.Claims;
using Artify.Api.Data;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Artify.Api.Enums;


namespace Artify.Api.Services.Implementations
{
    public class ArtistDashboardService : IArtistDashboardService
    {
        private readonly IArtworkRepository _artworkRepo;
        private readonly IArtServiceRepository _serviceRepo;
        private readonly IArtistRepository _artistRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IReviewRepository _reviewRepo;
        private readonly ApplicationDbContext _context;


        public ArtistDashboardService(
            IArtworkRepository artworkRepo,
            IArtServiceRepository serviceRepo,
            IArtistRepository artistRepo,
            IOrderRepository orderRepo,
            IReviewRepository reviewRepo,
            ApplicationDbContext context
)
        {
            _artworkRepo = artworkRepo;
            _serviceRepo = serviceRepo;
            _artistRepo = artistRepo;
            _orderRepo = orderRepo;
            _reviewRepo = reviewRepo;
            _context = context;

        }

        public async Task<object> GetSummaryAsync(ClaimsPrincipal user)
        {
            var artistId = _artistRepo.GetArtistId(user);

            var artworks = await _artworkRepo.GetAllByArtistAsync(artistId);
            var services = await _serviceRepo.GetAllByArtistAsync(artistId);
            var orders = await _orderRepo.GetAllByArtistAsync(artistId);
            var reviews = await _reviewRepo.GetAllByArtistAsync(artistId);

            return new
            {
                TotalArtworks = artworks?.Count() ?? 0,
                TotalServices = services?.Count() ?? 0,
                TotalOrders = orders?.Count() ?? 0,
                TotalReviews = reviews?.Count() ?? 0
            };
        }

        public async Task<object> GetOrdersAsync(ClaimsPrincipal user)
        {
            var artistId = _artistRepo.GetArtistId(user);

            var orders = await _orderRepo.GetAllByArtistAsync(artistId);
            return orders.Select(o => new
            {
                o.OrderId,
                Artwork = o.Artwork?.Title,
                o.Buyer,
                o.PaymentStatus,
                o.DeliveryStatus,
                o.OrderDate
            });
        }

        public async Task<object> GetReviewsAsync(ClaimsPrincipal user)
        {
            var artistId = _artistRepo.GetArtistId(user);

            var reviews = await _reviewRepo.GetAllByArtistAsync(artistId);
            return reviews.Select(r => new
            {
                r.ReviewId,
                r.Reviewer,
                r.Rating,
                r.Comment,
                r.CreatedAt
            });
        }

        public async Task<object> GetEarningsAsync(ClaimsPrincipal user)
        {
            var artistProfileId = Guid.Parse(
                user.FindFirstValue(ClaimTypes.NameIdentifier));

            var totalEarnings = await _context.Orders
                .Where(o =>
                    o.ArtistProfileId == artistProfileId &&
                    o.PaymentStatus == "Paid")
                .SumAsync(o => o.TotalAmount);

            return new { totalEarnings };
        }

        public async Task<object> GetArtworkStatsAsync(ClaimsPrincipal user)
        {
            var artistProfileId = Guid.Parse(
                user.FindFirstValue(ClaimTypes.NameIdentifier));

            var totalArtworks = await _context.Artworks
                .CountAsync(a => a.ArtistProfileId == artistProfileId);

            var soldArtworks = await _context.Orders
                .Where(o =>
                    o.ArtistProfileId == artistProfileId &&
                    o.PaymentStatus == "Paid")
                .Select(o => o.ArtworkId)
                .Distinct()
                .CountAsync();

            return new
            {
                totalArtworks,
                soldArtworks
            };
        }

    }
}
