using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IHiringRepository
    {
        // Hiring operations using Orders table with OrderType = "Hiring"
        Task<Order?> GetHiringRequestByIdAsync(Guid requestId);
        Task<IEnumerable<Order>> GetHiringRequestsByBuyerIdAsync(string buyerId);
        Task<Order> CreateHiringRequestAsync(Order hiringRequest);
        Task<bool> UpdateHiringRequestAsync(Order hiringRequest);
        Task<bool> DeleteHiringRequestAsync(Guid requestId);

        // Validation
        Task<bool> HiringRequestExistsAsync(Guid requestId);
        Task<bool> IsHiringRequestOwnerAsync(Guid requestId, string buyerId);
    }
}