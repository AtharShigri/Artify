using artifi.Api.DTOs.Admin;
using artifi.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace artifi.Api.Repositories.Interfaces
{
    public interface IAdminTransactionRepository
    {
        Task<IEnumerable<TransactionLog>> GetTransactionsAsync(TransactionFilterDto dto);
        Task<TransactionLog?> GetTransactionByIdAsync(Guid transactionId);
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order?> GetOrderByIdAsync(Guid orderId);
    }
}
