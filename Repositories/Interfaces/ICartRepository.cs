using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface ICartRepository
    {
        // Cart operations using Orders table with OrderType = "Cart"
        Task<Order?> GetCartByBuyerIdAsync(Guid buyerId);
        Task<Order> CreateCartAsync(Guid buyerId);
        Task<bool> AddToCartAsync(Guid buyerId, Guid artworkId);
        Task<bool> RemoveFromCartAsync(Guid buyerId, Guid artworkId);
        Task<bool> ClearCartAsync(Guid buyerId);
        Task<bool> CartExistsAsync(Guid buyerId);
    }
}