using Artify.Api.DTOs.Buyer;

namespace Artify.Api.Services.Interfaces
{
    public interface IOrderService
    {
        // Order Operations
        Task<OrderResponseDto> CreateOrderAsync(string buyerId, CreateOrderDto orderDto);
        Task<OrderResponseDto> GetOrderByIdAsync(Guid orderId, string buyerId);
        Task<IEnumerable<OrderResponseDto>> GetBuyerOrdersAsync(string buyerId);
        Task<bool> CancelOrderAsync(Guid orderId, string buyerId);

        // Order Status
        Task<bool> UpdateOrderStatusAsync(Guid orderId, string status);
        Task<string> GetOrderStatusAsync(Guid orderId);

        // Order Validation
        Task<bool> ValidateOrderItemsAsync(CreateOrderDto orderDto);
        Task<decimal> CalculateOrderTotalAsync(CreateOrderDto orderDto);
    }
}