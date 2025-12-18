using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Controllers.Buyer
{
    [Route("api/buyer/orders")]
    [ApiController]
    [Authorize(Roles = "Buyer")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(
            IOrderService orderService,
            ICartService cartService,
            ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _cartService = cartService;
            _logger = logger;
        }

        // Helper method to get current user ID
        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        /// <summary>
        /// Create a new order (from cart or direct purchase)
        /// </summary>
        /// <param name="orderDto">Order details</param>
        /// <returns>Created order</returns>
        [HttpPost]
        [ProducesResponseType(typeof(OrderResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                // Check if cart is empty
                var cartCount = await _cartService.GetCartItemCountAsync(buyerId);
                if (cartCount == 0 && (orderDto.Items == null || !orderDto.Items.Any()))
                    return BadRequest(new { message = "Cannot create empty order" });

                var order = await _orderService.CreateOrderAsync(buyerId, orderDto);
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get all orders for current buyer
        /// </summary>
        /// <returns>List of buyer's orders</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderResponseDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var orders = await _orderService.GetBuyerOrdersAsync(buyerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders");
                return StatusCode(500, new { message = "An error occurred while fetching orders" });
            }
        }

        /// <summary>
        /// Get specific order by ID
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns>Order details</returns>
        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(OrderResponseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var order = await _orderService.GetOrderByIdAsync(orderId, buyerId);
                if (order == null)
                    return NotFound(new { message = "Order not found" });

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order by ID");
                return StatusCode(500, new { message = "An error occurred while fetching order" });
            }
        }

        /// <summary>
        /// Cancel an order
        /// </summary>
        /// <param name="orderId">Order ID to cancel</param>
        /// <returns>Success message</returns>
        [HttpPut("cancel/{orderId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var result = await _orderService.CancelOrderAsync(orderId, buyerId);
                if (!result)
                    return BadRequest(new { message = "Cannot cancel this order" });

                return Ok(new { message = "Order cancelled successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get order status
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns>Order status</returns>
        [HttpGet("{orderId}/status")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetOrderStatus(Guid orderId)
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var order = await _orderService.GetOrderByIdAsync(orderId, buyerId);
                if (order == null)
                    return NotFound(new { message = "Order not found" });

                var status = await _orderService.GetOrderStatusAsync(orderId);
                return Ok(new { status });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order status");
                return StatusCode(500, new { message = "An error occurred while fetching order status" });
            }
        }

        /// <summary>
        /// Get order history with optional filters
        /// </summary>
        /// <param name="status">Filter by status (optional)</param>
        /// <param name="startDate">Filter by start date (optional)</param>
        /// <param name="endDate">Filter by end date (optional)</param>
        /// <returns>Filtered order history</returns>
        [HttpGet("history")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponseDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetOrderHistory(
            [FromQuery] string? status = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var buyerId = GetCurrentUserId();
                if (string.IsNullOrEmpty(buyerId))
                    return Unauthorized(new { message = "User not authenticated" });

                var allOrders = await _orderService.GetBuyerOrdersAsync(buyerId);

                // Apply filters
                var filteredOrders = allOrders.AsQueryable();

                if (!string.IsNullOrEmpty(status))
                {
                    filteredOrders = filteredOrders.Where(o => o.DeliveryStatus.Equals(status, StringComparison.OrdinalIgnoreCase));
                }

                if (startDate.HasValue)
                {
                    filteredOrders = filteredOrders.Where(o => o.OrderDate >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    filteredOrders = filteredOrders.Where(o => o.OrderDate <= endDate.Value);
                }

                return Ok(filteredOrders.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order history");
                return StatusCode(500, new { message = "An error occurred while fetching order history" });
            }
        }
    }
}