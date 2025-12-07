using AutoMapper;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IMapper _mapper;
        private readonly IBuyerRepository _buyerRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(
            IMapper mapper,
            IBuyerRepository buyerRepository,
            IPaymentRepository paymentRepository,
            IOrderRepository orderRepository)
        {
            _mapper = mapper;
            _buyerRepository = buyerRepository;
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<PaymentResponseDto> CreatePaymentIntentAsync(int orderId, string buyerId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null || order.BuyerId != buyerId)
                throw new Exception("Order not found or unauthorized");

            if (order.PaymentStatus != "Pending")
                throw new Exception($"Order payment status is {order.PaymentStatus}");

            var paymentIntentId = $"pi_{Guid.NewGuid():N}";

            var transaction = new TransactionLog
            {
                OrderId = orderId,
                PaymentMethod = "Stripe",
                TransactionAmount = order.TotalAmount,
                TransactionDate = DateTime.UtcNow,
                Status = "requires_payment_method"
            };

            await _paymentRepository.CreateTransactionAsync(transaction);

            // Update order with payment intent
            // We'll store payment intent ID in the Description field or handle differently

            return new PaymentResponseDto
            {
                ClientSecret = $"pi_{Guid.NewGuid():N}_secret_{Guid.NewGuid():N}",
                PaymentIntentId = paymentIntentId,
                Amount = order.TotalAmount,
                Status = "requires_payment_method"
            };
        }

        public async Task<PaymentResponseDto> ConfirmPaymentAsync(string paymentIntentId, int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new Exception("Order not found");

            // Update order status
            order.PaymentStatus = "Completed";
            order.DeliveryStatus = "Processing";
            await _orderRepository.UpdateOrderAsync(order);

            // Create transaction record
            var transaction = new TransactionLog
            {
                OrderId = orderId,
                PaymentMethod = "Stripe",
                TransactionAmount = order.TotalAmount,
                TransactionDate = DateTime.UtcNow,
                Status = "succeeded"
            };

            await _paymentRepository.CreateTransactionAsync(transaction);

            return new PaymentResponseDto
            {
                ClientSecret = "",
                PaymentIntentId = paymentIntentId,
                Amount = order.TotalAmount,
                Status = "succeeded"
            };
        }

        public async Task<bool> ProcessPaymentWebhookAsync(PaymentCallbackDto webhookDto)
        {
            // Find transaction by order ID
            var transactions = await _paymentRepository.GetBuyerTransactionsAsync("", 1, 100);
            var transaction = transactions.FirstOrDefault(t => t.OrderId == webhookDto.OrderId);

            if (transaction == null) return false;

            transaction.Status = webhookDto.Status;
            await _paymentRepository.UpdateTransactionAsync(transaction);

            // Update order status
            var order = await _orderRepository.GetOrderByIdAsync(webhookDto.OrderId);
            if (order != null)
            {
                order.PaymentStatus = webhookDto.Status;
                await _orderRepository.UpdateOrderAsync(order);
            }

            return true;
        }

        public async Task<string> GetPaymentStatusAsync(int orderId)
        {
            var transactions = await _paymentRepository.GetBuyerTransactionsAsync("", 1, 100);
            var transaction = transactions.FirstOrDefault(t => t.OrderId == orderId);
            return transaction?.Status ?? "unknown";
        }

        public async Task<bool> UpdatePaymentStatusAsync(int orderId, string status)
        {
            var transactions = await _paymentRepository.GetBuyerTransactionsAsync("", 1, 100);
            var transaction = transactions.FirstOrDefault(t => t.OrderId == orderId);
            if (transaction == null) return false;

            transaction.Status = status;
            await _paymentRepository.UpdateTransactionAsync(transaction);

            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                order.PaymentStatus = status;
                await _orderRepository.UpdateOrderAsync(order);
            }

            return true;
        }

        public async Task<IEnumerable<TransactionLogDto>> GetBuyerTransactionsAsync(string buyerId)
        {
            var transactions = await _paymentRepository.GetBuyerTransactionsAsync(buyerId);
            return transactions.Select(t => new TransactionLogDto
            {
                TransactionId = t.TransactionId,
                OrderId = t.OrderId,
                PaymentMethod = t.PaymentMethod,
                TransactionAmount = t.TransactionAmount,
                Status = t.Status,
                TransactionDate = t.TransactionDate
            });
        }

        public async Task<bool> ProcessRefundAsync(int orderId, decimal amount)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null || order.PaymentStatus != "Completed")
                return false;

            var refundTransaction = new TransactionLog
            {
                OrderId = orderId,
                PaymentMethod = "Stripe",
                TransactionAmount = -amount,
                TransactionDate = DateTime.UtcNow,
                Status = "refunded"
            };

            await _paymentRepository.CreateTransactionAsync(refundTransaction);

            order.PaymentStatus = "Refunded";
            await _orderRepository.UpdateOrderAsync(order);

            return true;
        }
    }
}