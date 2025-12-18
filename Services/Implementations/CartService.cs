using AutoMapper;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Services.Implementations
{
    public class CartService : BaseService, ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(
            IMapper mapper,
            IBuyerRepository buyerRepository,
            ICartRepository cartRepository)
            : base(mapper, buyerRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<CartResponseDto> GetCartAsync(string buyerId)
        {
            var cart = await _cartRepository.GetCartByBuyerIdAsync(buyerId);
            if (cart == null || cart.ArtworkId == null)
            {
                return new CartResponseDto { BuyerId = buyerId };
            }

            var cartDto = new CartResponseDto
            {
                BuyerId = buyerId,
                Items = new List<CartItemResponseDto>()
            };

            var artwork = await _buyerRepository.GetArtworkByIdAsync(cart.ArtworkId.Value);
            if (artwork != null)
            {
                cartDto.Items.Add(new CartItemResponseDto
                {
                    CartItemId = cart.OrderId,
                    ArtworkId = artwork.ArtworkId,
                    ArtworkTitle = artwork.Title,
                    ArtistName = artwork.ArtistProfile?.User?.FullName ?? "Unknown Artist",
                    Price = artwork.Price,
                    ImageUrl = artwork.ImageUrl,
                    Quantity = 1,
                    IsAvailable = artwork.IsForSale && artwork.Stock > 0
                });
            }

            return cartDto;
        }

        public async Task<CartResponseDto> AddToCartAsync(string buyerId, CartItemDto cartItem)
        {
            // Check if artwork exists and is available
            var artwork = await _buyerRepository.GetArtworkByIdAsync(cartItem.ArtworkId);
            if (artwork == null)
                throw new Exception("Artwork not found");

            if (!artwork.IsForSale)
                throw new Exception("Artwork is not for sale");

            if (artwork.Stock < cartItem.Quantity)
                throw new Exception("Insufficient stock");

            // Check if user already has this item in cart
            var existingCart = await _cartRepository.GetCartByBuyerIdAsync(buyerId);
            if (existingCart != null && existingCart.ArtworkId == cartItem.ArtworkId)
                throw new Exception("Artwork already in cart");

            // Remove existing cart item if exists
            if (existingCart != null && existingCart.ArtworkId != null)
            {
                await _cartRepository.RemoveFromCartAsync(buyerId, existingCart.ArtworkId.Value);
            }

            // Add to cart
            var added = await _cartRepository.AddToCartAsync(buyerId, cartItem.ArtworkId);
            if (!added)
                throw new Exception("Failed to add item to cart");

            return await GetCartAsync(buyerId);
        }

        public async Task<CartResponseDto> UpdateCartItemAsync(string buyerId, Guid artworkId, int quantity)
        {
            if (quantity <= 0)
            {
                await RemoveFromCartAsync(buyerId, artworkId);
                return await GetCartAsync(buyerId);
            }

            // In current implementation, we only support quantity of 1
            // Just validate the artwork is still available
            var artwork = await _buyerRepository.GetArtworkByIdAsync(artworkId);
            if (artwork == null || !artwork.IsForSale || artwork.Stock < quantity)
                throw new Exception("Artwork not available");

            return await GetCartAsync(buyerId);
        }

        public async Task<bool> RemoveFromCartAsync(string buyerId, Guid artworkId)
        {
            return await _cartRepository.RemoveFromCartAsync(buyerId, artworkId);
        }

        public async Task<bool> ClearCartAsync(string buyerId)
        {
            return await _cartRepository.ClearCartAsync(buyerId);
        }

        public async Task<bool> IsArtworkInCartAsync(string buyerId, Guid artworkId)
        {
            var cart = await _cartRepository.GetCartByBuyerIdAsync(buyerId);
            return cart != null && cart.ArtworkId == artworkId;
        }

        public async Task<int> GetCartItemCountAsync(string buyerId)
        {
            var cart = await _cartRepository.GetCartByBuyerIdAsync(buyerId);
            return cart?.ArtworkId != null ? 1 : 0;
        }

        public async Task<decimal> GetCartTotalAsync(string buyerId)
        {
            var cart = await _cartRepository.GetCartByBuyerIdAsync(buyerId);
            if (cart == null || cart.ArtworkId == null)
                return 0;

            var artwork = await _buyerRepository.GetArtworkByIdAsync(cart.ArtworkId.Value);
            return artwork?.Price ?? 0;
        }

        public async Task<CreateOrderDto> PrepareCheckoutAsync(string buyerId)
        {
            var cart = await GetCartAsync(buyerId);
            if (cart.Items.Count == 0)
                throw new Exception("Cart is empty");

            var orderDto = new CreateOrderDto
            {
                Items = cart.Items.Select(item => new OrderItemDto
                {
                    ArtworkId = item.ArtworkId,
                    Quantity = item.Quantity
                }).ToList(),
                ShippingAddress = "", // User will provide this during checkout
                PaymentMethod = "Stripe"
            };

            return orderDto;
        }
    }
}