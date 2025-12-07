using Artify.Api.Data;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Artify.Api.Repositories.Implementations
{
    public class CartRepository : BaseRepository, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Order?> GetCartByBuyerIdAsync(string buyerId)
        {
            return await _context.Orders
                .Include(o => o.Artwork)
                .ThenInclude(a => a!.ArtistProfile)
                .ThenInclude(ap => ap!.User)
                .FirstOrDefaultAsync(o => o.BuyerId == buyerId &&
                                        o.OrderType == "Cart" &&
                                        o.PaymentStatus == "Pending");
        }

        public async Task<Order> CreateCartAsync(string buyerId)
        {
            var cart = new Order
            {
                BuyerId = buyerId,
                OrderType = "Cart",
                PaymentStatus = "Pending",
                DeliveryStatus = "InCart",
                CreatedAt = DateTime.UtcNow,
                OrderDate = DateTime.UtcNow
            };

            await _context.Orders.AddAsync(cart);
            await SaveAsync();
            return cart;
        }

        public async Task<bool> AddToCartAsync(string buyerId, int artworkId)
        {
            var cart = await GetCartByBuyerIdAsync(buyerId);
            if (cart == null)
            {
                cart = await CreateCartAsync(buyerId);
            }

            // Check if artwork is already in cart
            if (cart.ArtworkId == artworkId)
                return false;

            var artwork = await _context.Artworks.FindAsync(artworkId);
            if (artwork == null || !artwork.IsForSale || artwork.Stock < 1)
                return false;

            cart.ArtworkId = artworkId;
            cart.TotalAmount = artwork.Price;
            cart.ArtistProfileId = artwork.ArtistProfileId;

            _context.Orders.Update(cart);
            return await SaveAsync();
        }

        public async Task<bool> RemoveFromCartAsync(string buyerId, int artworkId)
        {
            var cart = await GetCartByBuyerIdAsync(buyerId);
            if (cart == null || cart.ArtworkId != artworkId)
                return false;

            cart.ArtworkId = null;
            cart.TotalAmount = 0;
            cart.ArtistProfileId = 0;

            _context.Orders.Update(cart);
            return await SaveAsync();
        }

        public async Task<bool> ClearCartAsync(string buyerId)
        {
            var cart = await GetCartByBuyerIdAsync(buyerId);
            if (cart == null)
                return true;

            cart.ArtworkId = null;
            cart.TotalAmount = 0;
            cart.ArtistProfileId = 0;

            _context.Orders.Update(cart);
            return await SaveAsync();
        }

        public async Task<bool> CartExistsAsync(string buyerId)
        {
            return await _context.Orders
                .AnyAsync(o => o.BuyerId == buyerId &&
                              o.OrderType == "Cart" &&
                              o.PaymentStatus == "Pending");
        }
    }
}