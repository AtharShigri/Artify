using Artify.Api.DTOs.Buyer;

namespace Artify.Api.Services.Interfaces
{
    public interface IHiringService
    {
        // Hiring Operations (Using Orders table with OrderType = "Hiring")
        Task<HiringResponseDto> CreateHiringRequestAsync(string buyerId, HireArtistDto hireDto);
        Task<HiringResponseDto> GetHiringRequestAsync(Guid requestId, string buyerId);
        Task<IEnumerable<HiringResponseDto>> GetBuyerHiringRequestsAsync(string buyerId);
        Task<bool> DeleteHiringRequestAsync(Guid requestId, string buyerId);
        Task<HiringResponseDto> UpdateHiringRequestStatusAsync(Guid requestId, string status);

        // Hiring Validation
        Task<bool> IsArtistAvailableForHireAsync(int artistProfileId);
        Task<bool> ValidateHiringRequestAsync(HireArtistDto hireDto);

        // Communication
        Task<string> InitiateArtistCommunicationAsync(Guid requestId);
    }
}