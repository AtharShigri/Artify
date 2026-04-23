using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Claims;
using Artify.Api.Data;


namespace Artify.Api.Repositories.Implementations
{
    public class EscrowRepository : IEscrowRepository
    {
        private readonly ApplicationDbContext _context;

        public EscrowRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EscrowTransaction> GetByOrderIdAsync(Guid orderId)
        {
            return await _context.EscrowTransactions
                .FirstOrDefaultAsync(e => e.OrderId == orderId);
        }

        public async Task<EscrowTransaction> GetByIdAsync(Guid escrowId)
        {
            return await _context.EscrowTransactions.FindAsync(escrowId);
        }

        public async Task AddAsync(EscrowTransaction transaction)
        {
            await _context.EscrowTransactions.AddAsync(transaction);
        }

        public async Task UpdateAsync(EscrowTransaction transaction)
        {
            _context.EscrowTransactions.Update(transaction);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}