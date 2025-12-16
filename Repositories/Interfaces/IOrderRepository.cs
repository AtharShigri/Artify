using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllByArtistAsync(int artistId);
        Task<Order> GetByIdAsync(int orderId);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);
    }
}
        // Order operations
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByBuyerIdAsync(string buyerId);
        Task<Order> CreateOrderAsync(Order order);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> CancelOrderAsync(int orderId);

        // Validation
        Task<bool> OrderExistsAsync(int orderId);
        Task<bool> IsOrderOwnerAsync(int orderId, string buyerId);
    }
}
