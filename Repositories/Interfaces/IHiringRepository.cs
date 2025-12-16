using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IHiringRepository
    {
        // Hiring operations using Orders table with OrderType = "Hiring"
        Task<Order?> GetHiringRequestByIdAsync(int requestId);
        Task<IEnumerable<Order>> GetHiringRequestsByBuyerIdAsync(string buyerId);
        Task<Order> CreateHiringRequestAsync(Order hiringRequest);
        Task<bool> UpdateHiringRequestAsync(Order hiringRequest);
        Task<bool> DeleteHiringRequestAsync(int requestId);

        // Validation
        Task<bool> HiringRequestExistsAsync(int requestId);
        Task<bool> IsHiringRequestOwnerAsync(int requestId, string buyerId);
    }
}