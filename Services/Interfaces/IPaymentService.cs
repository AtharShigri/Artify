using Artify.Api.DTOs.Buyer;

namespace Artify.Api.Services.Interfaces
{
    public interface IPaymentService
    {
        // Payment Operations
        Task<PaymentResponseDto> CreatePaymentIntentAsync(int orderId, string buyerId);
        Task<PaymentResponseDto> ConfirmPaymentAsync(string paymentIntentId, int orderId);
        Task<bool> ProcessPaymentWebhookAsync(PaymentCallbackDto webhookDto);

        // Payment Status
        Task<string> GetPaymentStatusAsync(int orderId);
        Task<bool> UpdatePaymentStatusAsync(int orderId, string status);

        // Transaction History
        Task<IEnumerable<TransactionLogDto>> GetBuyerTransactionsAsync(string buyerId);

        // Refunds
        Task<bool> ProcessRefundAsync(int orderId, decimal amount);
    }

    public class TransactionLogDto
    {
        public int TransactionId { get; set; }
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal TransactionAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
    }
}