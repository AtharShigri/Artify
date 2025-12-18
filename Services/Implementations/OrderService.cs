using AutoMapper;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;
using System.Text.Json;

namespace Artify.Api.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IBuyerRepository _buyerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public OrderService(
            IMapper mapper,
            IBuyerRepository buyerRepository,
            IOrderRepository orderRepository,
            ICartRepository cartRepository)
        {
            _mapper = mapper;
            _buyerRepository = buyerRepository;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(string buyerId, CreateOrderDto orderDto)
        {
            // Validate order items
            if (!await ValidateOrderItemsAsync(orderDto))
                throw new Exception("One or more items are not available");

            // Calculate total
            var totalAmount = await CalculateOrderTotalAsync(orderDto);

            // Create order from cart or new order
            var cart = await _cartRepository.GetCartByBuyerIdAsync(buyerId);
            Order order;

            if (cart != null && cart.OrderType == "Cart")
            {
                // Convert cart to order
                order = cart;
                order.OrderType = "Purchase";
                order.PaymentStatus = "Pending";
                order.DeliveryStatus = "Processing";
                order.TotalAmount = totalAmount;
                order.OrderDate = DateTime.UtcNow;

                // Store shipping address in Description field since we don't have ShippingAddress column
                if (!string.IsNullOrEmpty(orderDto.ShippingAddress))
                {
                    // We'll store it in metadata or handle differently
                    // For now, we'll store in Description if it exists in model
                    // If not, we need to store it elsewhere
                }

                await _orderRepository.UpdateOrderAsync(order);
            }
            else
            {
                var firstItem = orderDto.Items.FirstOrDefault();
                if (firstItem == null)
                    throw new Exception("No items in order");

                var artwork = await _buyerRepository.GetArtworkByIdAsync(firstItem.ArtworkId);

                order = new Order
                {
                    BuyerId = buyerId,
                    OrderType = "Purchase",
                    TotalAmount = totalAmount,
                    PaymentStatus = "Pending",
                    DeliveryStatus = "Processing",
                    CreatedAt = DateTime.UtcNow,
                    OrderDate = DateTime.UtcNow,
                    ArtworkId = firstItem.ArtworkId,
                    ArtistProfileId = artwork?.ArtistProfileId ?? Guid.Empty
                };

                await _orderRepository.CreateOrderAsync(order);
            }

            // Clear cart after order creation
            if (cart != null)
            {
                await _cartRepository.ClearCartAsync(buyerId);
            }

            return await GetOrderByIdAsync(order.OrderId, buyerId);
        }

        public async Task<OrderResponseDto> GetOrderByIdAsync(Guid orderId, string buyerId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null || order.BuyerId != buyerId)
                return null;

            return await MapOrderToDto(order);
        }

        public async Task<IEnumerable<OrderResponseDto>> GetBuyerOrdersAsync(string buyerId)
        {
            var orders = await _orderRepository.GetOrdersByBuyerIdAsync(buyerId);
            var orderDtos = new List<OrderResponseDto>();

            foreach (var order in orders)
            {
                orderDtos.Add(await MapOrderToDto(order));
            }

            return orderDtos;
        }

        public async Task<bool> CancelOrderAsync(Guid orderId, string buyerId)
        {
            if (!await _orderRepository.IsOrderOwnerAsync(orderId, buyerId))
                return false;

            return await _orderRepository.CancelOrderAsync(orderId);
        }

        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, string status)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return false;

            order.DeliveryStatus = status;
            if (status == "Delivered")
            {
                order.CompletionDate = DateTime.UtcNow;
            }

            return await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task<string> GetOrderStatusAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            return order?.DeliveryStatus ?? "Not Found";
        }

        public async Task<bool> ValidateOrderItemsAsync(CreateOrderDto orderDto)
        {
            foreach (var item in orderDto.Items)
            {
                var artwork = await _buyerRepository.GetArtworkByIdAsync(item.ArtworkId);
                if (artwork == null || !artwork.IsForSale || artwork.Stock < item.Quantity)
                    return false;
            }
            return true;
        }

        public async Task<decimal> CalculateOrderTotalAsync(CreateOrderDto orderDto)
        {
            decimal total = 0;
            foreach (var item in orderDto.Items)
            {
                var artwork = await _buyerRepository.GetArtworkByIdAsync(item.ArtworkId);
                if (artwork != null)
                {
                    total += artwork.Price * item.Quantity;
                }
            }
            return total;
        }

        private async Task<OrderResponseDto> MapOrderToDto(Order order)
        {
            var dto = new OrderResponseDto
            {
                OrderId = order.OrderId,
                BuyerId = order.BuyerId,
                TotalAmount = order.TotalAmount,
                PaymentStatus = order.PaymentStatus,
                OrderType = order.OrderType,
                DeliveryStatus = order.DeliveryStatus,
                OrderDate = order.OrderDate,
                CompletionDate = order.CompletionDate,
                ShippingAddress = "", // Empty since we don't store it in database
                Items = new List<OrderItemResponseDto>()
            };

            if (order.ArtworkId != null)
            {
                var artwork = await _buyerRepository.GetArtworkByIdAsync(order.ArtworkId.Value);
                if (artwork != null)
                {
                    dto.Items.Add(new OrderItemResponseDto
                    {
                        ArtworkId = artwork.ArtworkId,
                        ArtworkTitle = artwork.Title,
                        ArtworkImage = artwork.ImageUrl,
                        ArtistName = artwork.ArtistProfile?.User?.FullName ?? "Unknown Artist",
                        Price = artwork.Price,
                        Quantity = 1
                    });
                }
            }

            return dto;
        }
    }
}