using Artify.Api.DTOs.Buyer;

namespace Artify.Api.Services.Interfaces
{
    public interface ICartService
    {
        // Cart Operations
        Task<CartResponseDto> GetCartAsync(Guid buyerId);
        Task<CartResponseDto> AddToCartAsync(Guid buyerId, CartItemDto cartItem);
        Task<CartResponseDto> UpdateCartItemAsync(Guid buyerId, Guid artworkId, int quantity);
        Task<bool> RemoveFromCartAsync(Guid buyerId, Guid artworkId);
        Task<bool> ClearCartAsync(Guid buyerId);

        // Cart Validation
        Task<bool> IsArtworkInCartAsync(Guid buyerId, Guid artworkId);
        Task<int> GetCartItemCountAsync(Guid buyerId);
        Task<decimal> GetCartTotalAsync(Guid buyerId);

        // Checkout Preparation
        Task<CreateOrderDto> PrepareCheckoutAsync(Guid buyerId);
    }
}