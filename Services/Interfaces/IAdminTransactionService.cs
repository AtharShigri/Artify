// ========================= IAdminTransactionService.cs =========================
using Artify.Api.DTOs.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artify.Api.Services.Interfaces
{
    public interface IAdminTransactionService
    {
        Task<IEnumerable<object>> GetTransactionsAsync(TransactionFilterDto dto);
        Task<object?> GetTransactionByIdAsync(int transactionId);
        Task<IEnumerable<object>> GetOrdersAsync();
        Task<object?> GetOrderByIdAsync(int orderId);
    }
}
