using Artify.Api.DTOs.Admin;
using Artify.Api.Mappings;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Services.Implementations
{
    public class AdminArtworkService : IAdminArtworkService
    {
        private readonly IAdminArtworkRepository _repository;
        private readonly INotificationService _notificationService;

        public AdminArtworkService(IAdminArtworkRepository repository, INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<object>> GetAllArtworksAsync()
        {
            var artworks = await _repository.GetAllArtworksAsync();
            return artworks.Select(AdminArtworkMappings.ToAdminArtworkDto);
        }

        public async Task<IEnumerable<object>> GetPendingArtworksAsync()
        {
            var artworks = await _repository.GetPendingArtworksAsync();
            return artworks.Select(AdminArtworkMappings.ToAdminArtworkDto);
        }

        public async Task<object?> GetArtworkByIdAsync(Guid artworkId)
        {
            var artwork = await _repository.GetArtworkByIdAsync(artworkId);
            return artwork == null ? null : AdminArtworkMappings.ToAdminArtworkDto(artwork);
        }

        public async Task<object> ApproveArtworkAsync(Guid artworkId)
        {
            var artwork = await _repository.GetArtworkByIdAsync(artworkId);
            if (artwork == null) throw new KeyNotFoundException("Artwork not found.");

            artwork.Status = "Published";
            artwork.IsApproved = true;
            var updated = await _repository.UpdateArtworkAsync(artwork);
            return AdminArtworkMappings.ToAdminArtworkDto(updated);
        }

        public async Task<object> RejectArtworkAsync(Guid artworkId, ArtworkModerationDto dto)
        {
            var artwork = await _repository.GetArtworkByIdAsync(artworkId);
            if (artwork == null) throw new KeyNotFoundException("Artwork not found.");

            artwork.Status = "Rejected";
            artwork.IsApproved = false;

            if (artwork.ArtistProfile != null)
            {
                await _notificationService.SendNotificationAsync(
                    artwork.ArtistProfile.UserId,
                    "Artwork Rejected",
                    $"Your artwork '{artwork.Title}' was rejected. Reason: {dto.RejectionReason ?? "Does not meet platform guidelines."}",
                    "Error"
                );
            }

            var updated = await _repository.UpdateArtworkAsync(artwork);
            return AdminArtworkMappings.ToAdminArtworkDto(updated);
        }

        public async Task<bool> RemoveArtworkAsync(Guid artworkId)
        {
            return await _repository.RemoveArtworkAsync(artworkId);
        }
    }
}
