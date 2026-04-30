using System.Threading.Tasks;
using artifi.Api.Models;

namespace artifi.Api.Repositories.Interfaces
{
    public interface IEscrowRepository
    {
        Task<EscrowTransaction> GetByOrderIdAsync(Guid orderId);
        Task<EscrowTransaction> GetByIdAsync(Guid escrowId);
        Task AddAsync(EscrowTransaction transaction);
        Task UpdateAsync(EscrowTransaction transaction);
        Task SaveChangesAsync();
    }
}