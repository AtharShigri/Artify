using AutoMapper;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;
using Artify.Api.DTOs.Shared;
using Microsoft.EntityFrameworkCore;
using Artify.Api.Data;

namespace Artify.Api.Services.Implementations
{
    public class HiringService : BaseService, IHiringService
    {
        private readonly IHiringRepository _hiringRepository;
        private readonly ApplicationDbContext _context;
        private readonly IChatService _chatService;
        private readonly INotificationService _notificationService;

        public HiringService(
            IMapper mapper,
            IBuyerRepository buyerRepository,
            IHiringRepository hiringRepository,
            ApplicationDbContext context,
            IChatService chatService,
            INotificationService notificationService)
            : base(mapper, buyerRepository)
        {
            _hiringRepository = hiringRepository;
            _context = context;
            _chatService = chatService;
            _notificationService = notificationService;
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

            // Notify artist
            try
            {
                var buyer = await _buyerRepository.GetBuyerByIdAsync(buyerId);
                await _notificationService.SendNotificationAsync(
                    artist.UserId,
                    "New Hiring Request",
                    $"{buyer?.FullName} wants to hire you for a project!",
                    "Success",
                    "/dashboard/artist/projects"
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to send notification: {ex.Message}");
            }

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

        public async Task<ConversationDto> InitiateArtistCommunicationAsync(Guid requestId, Guid buyerId)
        {
            var request = await _hiringRepository.GetHiringRequestByIdAsync(requestId);
            if (request == null || request.BuyerId != buyerId)
                throw new Exception("Hiring request not found");

            var artist = await _buyerRepository.GetArtistProfileByIdAsync(request.ArtistProfileId);
            if (artist == null)
                throw new Exception("Artist not found");

            // Create or get the conversation
            var conversation = await _chatService.GetOrCreateConversationAsync(buyerId, artist.UserId);
            
            return conversation;
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

            // Auto-send message to the buyer
            try
            {
                var artist = await _buyerRepository.GetArtistProfileByIdAsync(artistId);
                var conversation = await _chatService.GetOrCreateConversationAsync(request.BuyerId, artist.UserId);
                await _chatService.SaveAndProcessMessageAsync(
                    conversation.Id, 
                    artist.UserId, 
                    "I have accepted your hiring request! Let's discuss the details of the project."
                );

                // Notification
                await _notificationService.SendNotificationAsync(
                    request.BuyerId,
                    "Hiring Request Accepted",
                    $"{artist.User?.FullName} has accepted your project!",
                    "Success",
                    "/dashboard/buyer/projects"
                );
            }
            catch (Exception ex)
            {
                // Log and continue, we don't want to fail the acceptance if chat fails
                System.Diagnostics.Debug.WriteLine($"Failed to send auto-message/notification: {ex.Message}");
            }
        }

        public async Task RejectRequestAsync(Guid artistId, Guid requestId)
        {
            var request = await _hiringRepository.GetHiringRequestByIdAsync(requestId);

            if (request == null || request.ArtistProfileId != artistId || request.OrderType != "Hiring")
                throw new Exception("Hiring request not found");

            request.DeliveryStatus = "Rejected";
            await _hiringRepository.UpdateHiringRequestAsync(request);

            // Notify buyer
            try
            {
                var artist = await _buyerRepository.GetArtistProfileByIdAsync(artistId);
                await _notificationService.SendNotificationAsync(
                    request.BuyerId,
                    "Hiring Request Rejected",
                    $"{artist?.User?.FullName} has declined your hiring request.",
                    "Warning",
                    "/dashboard/buyer/projects"
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to send notification: {ex.Message}");
            }
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