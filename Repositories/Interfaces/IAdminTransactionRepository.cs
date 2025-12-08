// ========================= IAdminTransactionRepository.cs =========================
using Artify.Api.DTOs.Admin;
using Artify.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IAdminTransactionRepository
    {
        Task<IEnumerable<TransactionLog>> GetTransactionsAsync(TransactionFilterDto dto);
        Task<TransactionLog?> GetTransactionByIdAsync(int transactionId);
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int orderId);
    }
}
