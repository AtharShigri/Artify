using AutoMapper;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Artify.Api.Data;

namespace Artify.Api.Services.Implementations
{
    public class HiringService : BaseService, IHiringService
    {
        private readonly IHiringRepository _hiringRepository;
        private readonly ApplicationDbContext _context;

        public HiringService(
            IMapper mapper,
            IBuyerRepository buyerRepository,
            IHiringRepository hiringRepository,
            ApplicationDbContext context)
            : base(mapper, buyerRepository)
        {
            _hiringRepository = hiringRepository;
            _context = context;
        }

        // --- Buyer Actions ---

        public async Task<HiringResponseDto> CreateHiringRequestAsync(Guid buyerId, HireArtistDto hireDto)
        {
            if (!await ValidateHiringRequestAsync(hireDto))
                throw new Exception("Invalid hiring request");

            var artist = await _buyerRepository.GetArtistProfileByIdAsync(hireDto.ArtistProfileId);
            if (artist == null)
                throw new Exception("Artist not found");

            var hiringRequest = new Order
            {
                BuyerId = buyerId,
                ArtistProfileId = hireDto.ArtistProfileId,
                OrderType = "Hiring",
                TotalAmount = hireDto.Budget,
                PaymentStatus = "Pending",
                DeliveryStatus = "Requested",
                CreatedAt = DateTime.UtcNow,
                OrderDate = DateTime.UtcNow,
                CompletionDate = hireDto.Deadline
            };

            var createdRequest = await _hiringRepository.CreateHiringRequestAsync(hiringRequest);
            return await MapHiringRequestToDto(createdRequest, hireDto);
        }

        public async Task<HiringResponseDto?> GetHiringRequestAsync(Guid requestId, Guid buyerId)
        {
            var request = await _hiringRepository.GetHiringRequestByIdAsync(requestId);
            if (request == null || request.BuyerId != buyerId)
                return null;

            return await MapHiringRequestToDto(request);
        }

        public async Task<IEnumerable<HiringResponseDto>> GetBuyerHiringRequestsAsync(Guid buyerId)
        {
            var requests = await _hiringRepository.GetHiringRequestsByBuyerIdAsync(buyerId);
            var requestDtos = new List<HiringResponseDto>();

            foreach (var request in requests)
            {
                requestDtos.Add(await MapHiringRequestToDto(request));
            }

            return requestDtos;
        }

        public async Task<bool> DeleteHiringRequestAsync(Guid requestId, Guid buyerId)
        {
            if (!await _hiringRepository.IsHiringRequestOwnerAsync(requestId, buyerId))
                return false;

            return await _hiringRepository.DeleteHiringRequestAsync(requestId);
        }

        public async Task<string> InitiateArtistCommunicationAsync(Guid requestId, Guid buyerId)
        {
            var request = await _hiringRepository.GetHiringRequestByIdAsync(requestId);
            if (request == null || request.BuyerId != buyerId)
                throw new Exception("Hiring request not found");

            var artist = await _buyerRepository.GetArtistProfileByIdAsync(request.ArtistProfileId);
            if (artist == null)
                throw new Exception("Artist not found");

            return $"Communication initiated with artist: {artist.User?.FullName}. Use the in-app messaging system.";
        }

        // --- Artist Actions ---

        public async Task<IEnumerable<HiringResponseDto>> GetArtistRequestsAsync(Guid artistId)
        {
            var requests = await _hiringRepository.GetHiringRequestsByArtistIdAsync(artistId);
            var requestDtos = new List<HiringResponseDto>();

            foreach (var request in requests)
            {
                requestDtos.Add(await MapHiringRequestToDto(request));
            }

            return requestDtos;
        }

        public async Task AcceptRequestAsync(Guid artistId, Guid requestId)
        {
            var request = await _hiringRepository.GetHiringRequestByIdAsync(requestId);

            if (request == null || request.ArtistProfileId != artistId || request.OrderType != "Hiring")
                throw new Exception("Hiring request not found");

            request.DeliveryStatus = "Accepted";
            await _hiringRepository.UpdateHiringRequestAsync(request);
        }

        public async Task RejectRequestAsync(Guid artistId, Guid requestId)
        {
            var request = await _hiringRepository.GetHiringRequestByIdAsync(requestId);

            if (request == null || request.ArtistProfileId != artistId || request.OrderType != "Hiring")
                throw new Exception("Hiring request not found");

            request.DeliveryStatus = "Rejected";
            await _hiringRepository.UpdateHiringRequestAsync(request);
        }

        // --- Shared / Validation Logic ---

        public async Task<HiringResponseDto> UpdateHiringRequestStatusAsync(Guid requestId, string status)
        {
            var request = await _hiringRepository.GetHiringRequestByIdAsync(requestId);
            if (request == null)
                throw new Exception("Hiring request not found");

            request.DeliveryStatus = status;
            if (status == "Completed" || status == "Cancelled")
            {
                request.CompletionDate = DateTime.UtcNow;
            }

            await _hiringRepository.UpdateHiringRequestAsync(request);
            return await MapHiringRequestToDto(request);
        }

        public async Task<bool> IsArtistAvailableForHireAsync(Guid artistProfileId)
        {
            var artist = await _buyerRepository.GetArtistProfileByIdAsync(artistProfileId);
            return artist != null;
        }

        public async Task<bool> ValidateHiringRequestAsync(HireArtistDto hireDto)
        {
            if (hireDto.Deadline <= DateTime.UtcNow) return false;
            if (hireDto.Budget <= 0) return false;
            if (string.IsNullOrWhiteSpace(hireDto.ProjectTitle) || string.IsNullOrWhiteSpace(hireDto.ProjectDescription))
                return false;
            if (hireDto.ArtistProfileId == Guid.Empty) return false;

            return true;
        }

        private async Task<HiringResponseDto> MapHiringRequestToDto(Order request, HireArtistDto hireDto = null)
        {
            var dto = new HiringResponseDto
            {
                RequestId = request.OrderId,
                BuyerId = request.BuyerId,
                ArtistProfileId = request.ArtistProfileId,
                Budget = request.TotalAmount,
                Status = request.DeliveryStatus,
                CreatedAt = request.CreatedAt,
                Deadline = request.CompletionDate ?? DateTime.UtcNow.AddDays(30)
            };

            var buyer = await _buyerRepository.GetBuyerByIdAsync(request.BuyerId);
            if (buyer != null) dto.BuyerName = buyer.FullName;

            var artist = await _buyerRepository.GetArtistProfileByIdAsync(request.ArtistProfileId);
            if (artist?.User != null) dto.ArtistName = artist.User.FullName;

            if (hireDto != null)
            {
                dto.ProjectTitle = hireDto.ProjectTitle;
                dto.ProjectDescription = hireDto.ProjectDescription;
            }
            else
            {
                dto.ProjectTitle = "Art Commission";
                dto.ProjectDescription = "Custom artwork commission";
            }

            return dto;
        }
    }
}