using Artify.Api.Data;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Artify.Api.Repositories.Implementations
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.Artwork)
                .ThenInclude(a => a!.ArtistProfile)
                .ThenInclude(ap => ap!.User)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByBuyerIdAsync(string buyerId)
        {
            return await _context.Orders
                .Where(o => o.BuyerId == buyerId &&
                           o.OrderType != "Cart" &&
                           o.PaymentStatus != "Pending")
                .OrderByDescending(o => o.CreatedAt)
                .Include(o => o.Artwork)
                .ThenInclude(a => a!.ArtistProfile)
                .ThenInclude(ap => ap!.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllByArtistAsync(int artistProfileId)
        {
            return await _context.Orders
                .Where(o => o.ArtistProfileId == artistProfileId)
                .OrderByDescending(o => o.CreatedAt)
                .Include(o => o.Artwork)
                .ThenInclude(a => a.ArtistProfile)
                .ToListAsync();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await SaveAsync();
            return order;
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            return await SaveAsync();
        }

        public async Task<bool> CancelOrderAsync(Guid orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order == null)
                return false;

            if (order.PaymentStatus == "Completed" || order.DeliveryStatus == "Shipped")
                return false;

            order.PaymentStatus = "Cancelled";
            order.DeliveryStatus = "Cancelled";

            _context.Orders.Update(order);
            return await SaveAsync();
        }

        public async Task<bool> OrderExistsAsync(Guid orderId)
        {
            return await _context.Orders.AnyAsync(o => o.OrderId == orderId);
        }

        public async Task<bool> IsOrderOwnerAsync(Guid orderId, string buyerId)
        {
            return await _context.Orders
                .AnyAsync(o => o.OrderId == orderId && o.BuyerId == buyerId);
        }
    }
}