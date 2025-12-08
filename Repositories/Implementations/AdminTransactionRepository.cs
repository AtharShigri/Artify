// ========================= AdminTransactionRepository.cs =========================
using Artify.Api.Data;
using Artify.Api.DTOs.Admin;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artify.Api.Repositories.Implementations
{
    public class AdminTransactionRepository : IAdminTransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminTransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TransactionLog>> GetTransactionsAsync(TransactionFilterDto dto)
        {
            var query = _context.TransactionLogs.AsQueryable();

            if (dto.FromDate.HasValue)
                query = query.Where(t => t.TransactionDate >= dto.FromDate.Value);

            if (dto.ToDate.HasValue)
                query = query.Where(t => t.TransactionDate <= dto.ToDate.Value);

            if (!string.IsNullOrEmpty(dto.Status))
                query = query.Where(t => t.Status == dto.Status);

            if (!string.IsNullOrEmpty(dto.PaymentMethod))
                query = query.Where(t => t.PaymentMethod == dto.PaymentMethod);

            return await query
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<TransactionLog?> GetTransactionByIdAsync(int transactionId)
        {
            return await _context.TransactionLogs
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }
    }
}
