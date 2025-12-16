using AutoMapper;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Services.Implementations
{
    public class HiringService : BaseService, IHiringService
    {
        private readonly IHiringRepository _hiringRepository;

        public HiringService(
            IMapper mapper,
            IBuyerRepository buyerRepository,
            IHiringRepository hiringRepository)
            : base(mapper, buyerRepository)
        {
            _hiringRepository = hiringRepository;
        }

        public async Task<HiringResponseDto> CreateHiringRequestAsync(string buyerId, HireArtistDto hireDto)
        {
            // Validate hiring request
            if (!await ValidateHiringRequestAsync(hireDto))
                throw new Exception("Invalid hiring request");

            // Check if artist exists
            var artist = await _buyerRepository.GetArtistProfileByIdAsync(hireDto.ArtistProfileId);
            if (artist == null)
                throw new Exception("Artist not found");

            // Create hiring request using Orders table
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

        public async Task<HiringResponseDto> GetHiringRequestAsync(int requestId, string buyerId)
        {
            var request = await _hiringRepository.GetHiringRequestByIdAsync(requestId);
            if (request == null || request.BuyerId != buyerId)
                return null;

            return await MapHiringRequestToDto(request);
        }

        public async Task<IEnumerable<HiringResponseDto>> GetBuyerHiringRequestsAsync(string buyerId)
        {
            var requests = await _hiringRepository.GetHiringRequestsByBuyerIdAsync(buyerId);
            var requestDtos = new List<HiringResponseDto>();

            foreach (var request in requests)
            {
                requestDtos.Add(await MapHiringRequestToDto(request));
            }

            return requestDtos;
        }

        public async Task<bool> DeleteHiringRequestAsync(int requestId, string buyerId)
        {
            if (!await _hiringRepository.IsHiringRequestOwnerAsync(requestId, buyerId))
                return false;

            return await _hiringRepository.DeleteHiringRequestAsync(requestId);
        }

        public async Task<HiringResponseDto> UpdateHiringRequestStatusAsync(int requestId, string status)
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

        public async Task<bool> IsArtistAvailableForHireAsync(int artistProfileId)
        {
            var artist = await _buyerRepository.GetArtistProfileByIdAsync(artistProfileId);
            return artist != null;
        }

        public async Task<bool> ValidateHiringRequestAsync(HireArtistDto hireDto)
        {
            if (hireDto.Deadline <= DateTime.UtcNow)
                return false;

            if (hireDto.Budget <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(hireDto.ProjectTitle) ||
                string.IsNullOrWhiteSpace(hireDto.ProjectDescription))
                return false;

            if (hireDto.ArtistProfileId <= 0)
                return false;

            return true;
        }

        public async Task<string> InitiateArtistCommunicationAsync(int requestId)
        {
            var request = await _hiringRepository.GetHiringRequestByIdAsync(requestId);
            if (request == null)
                throw new Exception("Hiring request not found");

            var artist = await _buyerRepository.GetArtistProfileByIdAsync(request.ArtistProfileId);
            if (artist == null)
                throw new Exception("Artist not found");

            return $"Communication initiated with artist: {artist.User?.FullName}. Use the in-app messaging system.";
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

            // Get buyer details
            var buyer = await _buyerRepository.GetBuyerByIdAsync(request.BuyerId);
            if (buyer != null)
            {
                dto.BuyerName = buyer.FullName;
            }

            // Get artist details
            var artist = await _buyerRepository.GetArtistProfileByIdAsync(request.ArtistProfileId);
            if (artist != null && artist.User != null)
            {
                dto.ArtistName = artist.User.FullName;
            }

            // If we have the original hireDto, use its details
            if (hireDto != null)
            {
                dto.ProjectTitle = hireDto.ProjectTitle;
                dto.ProjectDescription = hireDto.ProjectDescription;
                dto.Budget = hireDto.Budget;
                dto.Deadline = hireDto.Deadline;
            }
            else
            {
                // Use defaults
                dto.ProjectTitle = "Art Commission";
                dto.ProjectDescription = "Custom artwork commission";
            }

            return dto;
        }
    }
}