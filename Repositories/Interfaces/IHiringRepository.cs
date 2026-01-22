using Artify.Api.Models;

namespace Artify.Api.Repositories.Interfaces
{
    public interface IHiringRepository
    {
        // Retrieval
        Task<Order?> GetHiringRequestByIdAsync(Guid requestId);
        Task<IEnumerable<Order>> GetHiringRequestsByBuyerIdAsync(Guid buyerId);
        Task<IEnumerable<Order>> GetHiringRequestsByArtistIdAsync(Guid artistId);

        // State Changes
        Task<Order> CreateHiringRequestAsync(Order hiringRequest);
        Task<bool> UpdateHiringRequestAsync(Order hiringRequest);
        Task<bool> DeleteHiringRequestAsync(Guid requestId);

        // Validation Helpers
        Task<bool> HiringRequestExistsAsync(Guid requestId);
        Task<bool> IsHiringRequestOwnerAsync(Guid requestId, Guid buyerId);
        Task<bool> IsHiringRequestRecipientAsync(Guid requestId, Guid artistId);
    }
}