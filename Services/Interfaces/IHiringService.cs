using artifi.Api.DTOs.Buyer;
using artifi.Api.DTOs.Shared;

namespace artifi.Api.Services.Interfaces
{
    public interface IHiringService
    {
        // --- Buyer Actions ---
        Task<HiringResponseDto> CreateHiringRequestAsync(Guid buyerId, HireArtistDto hireDto);
        Task<HiringResponseDto?> GetHiringRequestAsync(Guid requestId, Guid buyerId);
        Task<IEnumerable<HiringResponseDto>> GetBuyerHiringRequestsAsync(Guid buyerId);
        Task<bool> DeleteHiringRequestAsync(Guid requestId, Guid buyerId);
        Task<ConversationDto> InitiateArtistCommunicationAsync(Guid requestId, Guid buyerId);

        // --- Artist Actions ---
        Task<IEnumerable<HiringResponseDto>> GetArtistRequestsAsync(Guid artistId);
        Task AcceptRequestAsync(Guid artistId, Guid requestId);
        Task RejectRequestAsync(Guid artistId, Guid requestId);

        // --- Shared / Internal Logic ---
        Task<HiringResponseDto> UpdateHiringRequestStatusAsync(Guid requestId, string status);
        Task<bool> IsArtistAvailableForHireAsync(Guid artistProfileId);
        Task<bool> ValidateHiringRequestAsync(HireArtistDto hireDto);
    }
}