using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        // Payment Operations
        Task<TransactionLog?> GetTransactionByIdAsync(Guid transactionId);
        Task<TransactionLog?> GetTransactionByOrderIdAsync(Guid orderId);
        Task<TransactionLog> CreateTransactionAsync(TransactionLog transaction);
        Task<bool> UpdateTransactionAsync(TransactionLog transaction);

        // Order-Payment Linking
        Task<bool> LinkPaymentToOrderAsync(Guid orderId, string paymentIntentId, string status);
        Task<string?> GetOrderPaymentStatusAsync(Guid orderId);

        // Transaction History
        Task<IEnumerable<TransactionLog>> GetBuyerTransactionsAsync(Guid buyerId, int page = 1, int pageSize = 20);
    }
}