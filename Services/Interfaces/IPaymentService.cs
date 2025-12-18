using Artify.Api.DTOs.Buyer;

namespace Artify.Api.Services.Interfaces
{
    public interface IPaymentService
    {
        // Payment Operations
        Task<PaymentResponseDto> CreatePaymentIntentAsync(Guid orderId, string buyerId);
        Task<PaymentResponseDto> ConfirmPaymentAsync(string paymentIntentId, Guid orderId);
        Task<bool> ProcessPaymentWebhookAsync(PaymentCallbackDto webhookDto);

        // Payment Status
        Task<string> GetPaymentStatusAsync(Guid orderId);
        Task<bool> UpdatePaymentStatusAsync(Guid orderId, string status);

        // Transaction History
        Task<IEnumerable<TransactionLogDto>> GetBuyerTransactionsAsync(string buyerId);

        // Refunds
        Task<bool> ProcessRefundAsync(Guid orderId, decimal amount);
    }

    public class TransactionLogDto
    {
        public Guid TransactionId { get; set; }
        public Guid OrderId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal TransactionAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
    }
}