using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Controllers.Buyer
{
    [Route("api/buyer/cart")]
    [ApiController]
    [Authorize(Roles = "Buyer")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        private Guid? GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userId, out var guid) ? guid : null;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                var cart = await _cartService.GetCartAsync(buyerId.Value);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart");
                return StatusCode(500, new { message = "An error occurred while fetching cart" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDto cartItem)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var buyerId = GetCurrentUserId();
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                if (cartItem.Quantity <= 0)
                    return BadRequest(new { message = "Quantity must be greater than 0" });

                var cart = await _cartService.AddToCartAsync(buyerId.Value, cartItem);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{artworkId}")]
        public async Task<IActionResult> UpdateCartItem(Guid artworkId, [FromBody] int quantity)
        {
            try
            {
                if (quantity <= 0)
                    return BadRequest(new { message = "Quantity must be greater than 0" });

                var buyerId = GetCurrentUserId();
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                var cart = await _cartService.UpdateCartItemAsync(
                    buyerId.Value, artworkId, quantity);

                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{artworkId}")]
        public async Task<IActionResult> RemoveFromCart(Guid artworkId)
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                var result = await _cartService.RemoveFromCartAsync(
                    buyerId.Value, artworkId);

                if (!result)
                    return NotFound(new { message = "Item not found in cart" });

                return Ok(new { message = "Item removed from cart" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from cart");
                return StatusCode(500, new { message = "An error occurred while removing item from cart" });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                var result = await _cartService.ClearCartAsync(buyerId.Value);
                if (!result)
                    return BadRequest(new { message = "Failed to clear cart" });

                return Ok(new { message = "Cart cleared successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart");
                return StatusCode(500, new { message = "An error occurred while clearing cart" });
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCartItemCount()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                var count = await _cartService.GetCartItemCountAsync(buyerId.Value);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart item count");
                return StatusCode(500, new { message = "An error occurred while fetching cart count" });
            }
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetCartTotal()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                var total = await _cartService.GetCartTotalAsync(buyerId.Value);
                return Ok(total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart total");
                return StatusCode(500, new { message = "An error occurred while fetching cart total" });
            }
        }

        [HttpGet("checkout")]
        public async Task<IActionResult> PrepareCheckout()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (buyerId == null)
                    return Unauthorized(new { message = "User not authenticated" });

                var checkout = await _cartService.PrepareCheckoutAsync(buyerId.Value);
                return Ok(checkout);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error preparing checkout");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
