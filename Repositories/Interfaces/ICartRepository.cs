using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface ICartRepository
    {
        // Cart operations using Orders table with OrderType = "Cart"
        Task<Order?> GetCartByBuyerIdAsync(string buyerId);
        Task<Order> CreateCartAsync(string buyerId);
        Task<bool> AddToCartAsync(string buyerId, Guid artworkId);
        Task<bool> RemoveFromCartAsync(string buyerId, Guid artworkId);
        Task<bool> ClearCartAsync(string buyerId);
        Task<bool> CartExistsAsync(string buyerId);
    }
}