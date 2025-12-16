using Artify.Api.DTOs.Buyer;

namespace Artify.Api.Services.Interfaces
{
    public interface IHiringService
    {
        // Hiring Operations (Using Orders table with OrderType = "Hiring")
        Task<HiringResponseDto> CreateHiringRequestAsync(string buyerId, HireArtistDto hireDto);
        Task<HiringResponseDto> GetHiringRequestAsync(int requestId, string buyerId);
        Task<IEnumerable<HiringResponseDto>> GetBuyerHiringRequestsAsync(string buyerId);
        Task<bool> DeleteHiringRequestAsync(int requestId, string buyerId);
        Task<HiringResponseDto> UpdateHiringRequestStatusAsync(int requestId, string status);

        // Hiring Validation
        Task<bool> IsArtistAvailableForHireAsync(int artistProfileId);
        Task<bool> ValidateHiringRequestAsync(HireArtistDto hireDto);

        // Communication
        Task<string> InitiateArtistCommunicationAsync(int requestId);
    }
}