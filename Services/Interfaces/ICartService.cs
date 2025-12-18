using Artify.Api.DTOs.Buyer;

namespace Artify.Api.Services.Interfaces
{
    public interface ICartService
    {
        // Cart Operations
        Task<CartResponseDto> GetCartAsync(string buyerId);
        Task<CartResponseDto> AddToCartAsync(string buyerId, CartItemDto cartItem);
        Task<CartResponseDto> UpdateCartItemAsync(string buyerId, Guid artworkId, int quantity);
        Task<bool> RemoveFromCartAsync(string buyerId, Guid artworkId);
        Task<bool> ClearCartAsync(string buyerId);

        // Cart Validation
        Task<bool> IsArtworkInCartAsync(string buyerId, Guid artworkId);
        Task<int> GetCartItemCountAsync(string buyerId);
        Task<decimal> GetCartTotalAsync(string buyerId);

        // Checkout Preparation
        Task<CreateOrderDto> PrepareCheckoutAsync(string buyerId);
    }
}