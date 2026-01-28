using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IOrderRepository
    { 
        Task<Order?> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<Order>> GetOrdersByBuyerIdAsync(Guid buyerId);
        Task<IEnumerable<Order>> GetAllByArtistAsync(Guid artistProfileId);

        Task<Order> CreateOrderAsync(Order order);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> CancelOrderAsync(Guid orderId);

        Task<bool> OrderExistsAsync(Guid orderId);
        Task<bool> IsOrderOwnerAsync(Guid orderId, Guid buyerId);
    }
}
