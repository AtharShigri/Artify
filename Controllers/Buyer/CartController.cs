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

        // Helper method to get current user ID
        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        /// <summary>
        /// Get current buyer's shopping cart
        /// </summary>
        /// <returns>Shopping cart with items</returns>
        [HttpGet]
        [ProducesResponseType(typeof(CartResponseDto), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var cart = await _cartService.GetCartAsync(buyerId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart for user");
                return StatusCode(500, new { message = "An error occurred while fetching cart" });
            }
        }

        /// <summary>
        /// Add item to cart
        /// </summary>
        /// <param name="cartItem">Cart item details</param>
        /// <returns>Updated cart</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CartResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDto cartItem)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                if (cartItem.Quantity <= 0)
                    return BadRequest(new { message = "Quantity must be greater than 0" });

                var cart = await _cartService.AddToCartAsync(buyerId, cartItem);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update cart item quantity
        /// </summary>
        /// <param name="artworkId">Artwork ID</param>
        /// <param name="quantity">New quantity</param>
        /// <returns>Updated cart</returns>
        [HttpPut("{artworkId}")]
        [ProducesResponseType(typeof(CartResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCartItem(Guid artworkId, [FromBody] int quantity)
        {
            try
            {
                if (quantity <= 0)
                    return BadRequest(new { message = "Quantity must be greater than 0" });

                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var cart = await _cartService.UpdateCartItemAsync(buyerId, artworkId, quantity);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Remove item from cart
        /// </summary>
        /// <param name="artworkId">Artwork ID to remove</param>
        /// <returns>Success message</returns>
        [HttpDelete("{artworkId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveFromCart(Guid artworkId)
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var result = await _cartService.RemoveFromCartAsync(buyerId, artworkId);
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

        /// <summary>
        /// Clear entire cart
        /// </summary>
        /// <returns>Success message</returns>
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var result = await _cartService.ClearCartAsync(buyerId);
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

        /// <summary>
        /// Get cart item count
        /// </summary>
        /// <returns>Number of items in cart</returns>
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetCartItemCount()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var count = await _cartService.GetCartItemCountAsync(buyerId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart item count");
                return StatusCode(500, new { message = "An error occurred while fetching cart count" });
            }
        }

        /// <summary>
        /// Get cart total amount
        /// </summary>
        /// <returns>Total cart value</returns>
        [HttpGet("total")]
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetCartTotal()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var total = await _cartService.GetCartTotalAsync(buyerId);
                return Ok(total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart total");
                return StatusCode(500, new { message = "An error occurred while fetching cart total" });
            }
        }

        /// <summary>
        /// Prepare checkout (convert cart to order)
        /// </summary>
        /// <returns>Checkout details</returns>
        [HttpGet("checkout")]
        [ProducesResponseType(typeof(CreateOrderDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> PrepareCheckout()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var checkout = await _cartService.PrepareCheckoutAsync(buyerId);
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