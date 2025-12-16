using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        // Payment Operations
        Task<TransactionLog?> GetTransactionByIdAsync(int transactionId);
        Task<TransactionLog> CreateTransactionAsync(TransactionLog transaction);
        Task<bool> UpdateTransactionAsync(TransactionLog transaction);

        // Order-Payment Linking
        Task<bool> LinkPaymentToOrderAsync(int orderId, string paymentIntentId, string status);
        Task<string?> GetOrderPaymentStatusAsync(int orderId);

        // Transaction History
        Task<IEnumerable<TransactionLog>> GetBuyerTransactionsAsync(string buyerId, int page = 1, int pageSize = 20);
    }
}