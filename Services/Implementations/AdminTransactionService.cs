// ========================= AdminTransactionService.cs =========================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artify.Api.DTOs.Admin;
using Artify.Api.Mappings;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Services.Implementations
{
    public class AdminTransactionService : IAdminTransactionService
    {
        private readonly IAdminTransactionRepository _repository;

        public AdminTransactionService(IAdminTransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<object>> GetTransactionsAsync(TransactionFilterDto dto)
        {
            var transactions = await _repository.GetTransactionsAsync(dto);
            return transactions.Select(AdminTransactionMappings.ToTransactionDto).ToList();
        }

        public async Task<object?> GetTransactionByIdAsync(int transactionId)
        {
            TransactionLog? transaction = await _repository.GetTransactionByIdAsync(transactionId);
            if (transaction == null) return null;

            return AdminTransactionMappings.ToTransactionDto(transaction);
        }

        public async Task<IEnumerable<object>> GetOrdersAsync()
        {
            var orders = await _repository.GetOrdersAsync();
            return orders.Select(AdminTransactionMappings.ToOrderDto).ToList();
        }

        public async Task<object?> GetOrderByIdAsync(int orderId)
        {
            Order? order = await _repository.GetOrderByIdAsync(orderId);
            if (order == null) return null;

            return AdminTransactionMappings.ToOrderDto(order);
        }
    }
}
