using Artify.Api.Data;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Artify.Api.Repositories.Implementations
{
    public class TransactionRepository : BaseRepository, IPaymentRepository
    {
        public TransactionRepository(ApplicationDbContext context) : base(context) { }

        // 1. Get Transaction by ID
        public async Task<TransactionLog?> GetTransactionByIdAsync(int transactionId)
        {
            return await _context.TransactionLogs
                .Include(t => t.Order)
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        }

        // 2. Create Transaction
        public async Task<TransactionLog> CreateTransactionAsync(TransactionLog transaction)
        {
            await _context.TransactionLogs.AddAsync(transaction);
            await SaveAsync();
            return transaction;
        }

        // 3. Update Transaction
        public async Task<bool> UpdateTransactionAsync(TransactionLog transaction)
        {
            _context.TransactionLogs.Update(transaction);
            return await SaveAsync();
        }

        // 4. Get Transaction by Order ID
        public async Task<TransactionLog?> GetTransactionByOrderIdAsync(int orderId)
        {
            return await _context.TransactionLogs
                .FirstOrDefaultAsync(t => t.OrderId == orderId);
        }

        // 5. Link Payment to Order
        public async Task<bool> LinkPaymentToOrderAsync(int orderId, string paymentIntentId, string status)
        {
            var transaction = await GetTransactionByOrderIdAsync(orderId);
            if (transaction == null)
            {
                transaction = new TransactionLog
                {
                    OrderId = orderId,
                    PaymentMethod = "Stripe",
                    TransactionAmount = 0,
                    TransactionDate = DateTime.UtcNow,
                    Status = status
                };
                await CreateTransactionAsync(transaction);
            }
            else
            {
                transaction.Status = status;
                await UpdateTransactionAsync(transaction);
            }

            // Update order payment status
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.PaymentStatus = status;
                _context.Orders.Update(order);
                return await SaveAsync();
            }

            return false;
        }

        // 6. Get Order Payment Status
        public async Task<string?> GetOrderPaymentStatusAsync(int orderId)
        {
            var transaction = await GetTransactionByOrderIdAsync(orderId);
            return transaction?.Status;
        }

        // 7. Get Buyer Transactions with Paging
        public async Task<IEnumerable<TransactionLog>> GetBuyerTransactionsAsync(string buyerId, int page = 1, int pageSize = 20)
        {
            return await _context.TransactionLogs
                .Include(t => t.Order)
                .Where(t => t.Order != null && t.Order.BuyerId == buyerId)
                .OrderByDescending(t => t.TransactionDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}