using Artify.Api.DTOs.Buyer;
using System.Security.Claims;

namespace Artify.Api.Services.Interfaces
{
    public interface IOrderService
    {
        // Order Operations
        Task<OrderResponseDto> CreateOrderAsync(Guid buyerId, CreateOrderDto orderDto);
        Task<OrderResponseDto> GetOrderByIdAsync(Guid orderId, Guid buyerId);
        Task<IEnumerable<OrderResponseDto>> GetBuyerOrdersAsync(Guid buyerId);
        Task<bool> CancelOrderAsync(Guid orderId, Guid buyerId);

        // Order Status
        Task<bool> UpdateOrderStatusAsync(Guid orderId, string status);
        Task<string> GetOrderStatusAsync(Guid orderId);

        // Order Validation
        Task<bool> ValidateOrderItemsAsync(CreateOrderDto orderDto);
        Task<decimal> CalculateOrderTotalAsync(CreateOrderDto orderDto);
        Task<bool> CanReviewAsync(ClaimsPrincipal user, Guid orderId);

    }
}