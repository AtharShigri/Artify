using Artify.Api.Data;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Artify.Api.Repositories.Implementations
{
    public class HiringRepository : BaseRepository, IHiringRepository
    {
        public HiringRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Order?> GetHiringRequestByIdAsync(Guid requestId)
        {
            return await _context.Orders
                .Include(o => o.ArtistProfile)
                .ThenInclude(ap => ap!.User)
                .FirstOrDefaultAsync(o => o.OrderId == requestId &&
                                        o.OrderType == "Hiring");
        }

        public async Task<IEnumerable<Order>> GetHiringRequestsByBuyerIdAsync(string buyerId)
        {
            return await _context.Orders
                .Where(o => o.BuyerId == buyerId && o.OrderType == "Hiring")
                .OrderByDescending(o => o.CreatedAt)
                .Include(o => o.ArtistProfile)
                .ThenInclude(ap => ap!.User)
                .ToListAsync();
        }

        public async Task<Order> CreateHiringRequestAsync(Order hiringRequest)
        {
            hiringRequest.OrderType = "Hiring";
            hiringRequest.PaymentStatus = "Pending";
            hiringRequest.DeliveryStatus = "Requested";
            hiringRequest.CreatedAt = DateTime.UtcNow;
            hiringRequest.OrderDate = DateTime.UtcNow;

            await _context.Orders.AddAsync(hiringRequest);
            await SaveAsync();
            return hiringRequest;
        }

        public async Task<bool> UpdateHiringRequestAsync(Order hiringRequest)
        {
            _context.Orders.Update(hiringRequest);
            return await SaveAsync();
        }

        public async Task<bool> DeleteHiringRequestAsync(Guid requestId)
        {
            var request = await GetHiringRequestByIdAsync(requestId);
            if (request == null)
                return false;

            if (request.PaymentStatus == "Completed" ||
                request.DeliveryStatus == "Accepted")
                return false;

            _context.Orders.Remove(request);
            return await SaveAsync();
        }

        public async Task<bool> HiringRequestExistsAsync(Guid requestId)
        {
            return await _context.Orders
                .AnyAsync(o => o.OrderId == requestId && o.OrderType == "Hiring");
        }

        public async Task<bool> IsHiringRequestOwnerAsync(Guid requestId, string buyerId)
        {
            return await _context.Orders
                .AnyAsync(o => o.OrderId == requestId &&
                             o.BuyerId == buyerId &&
                             o.OrderType == "Hiring");
        }
    }
}