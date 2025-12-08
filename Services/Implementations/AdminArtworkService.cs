// ========================= AdminArtworkService.cs =========================
using Artify.Api.DTOs.Admin;
using Artify.Api.Mappings;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Services.Implementations
{
    public class AdminArtworkService : IAdminArtworkService
    {
        private readonly IAdminArtworkRepository _repository;

        public AdminArtworkService(IAdminArtworkRepository repository)
        {
            _repository = repository;
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

        public async Task<object?> GetArtworkByIdAsync(int artworkId)
        {
            var artwork = await _repository.GetArtworkByIdAsync(artworkId);
            return artwork == null ? null : AdminArtworkMappings.ToAdminArtworkDto(artwork);
        }

        public async Task<object> ApproveArtworkAsync(int artworkId)
        {
            var artwork = await _repository.GetArtworkByIdAsync(artworkId);
            if (artwork == null) throw new KeyNotFoundException("Artwork not found.");

            artwork.Status = "Approved";
            var updated = await _repository.UpdateArtworkAsync(artwork);
            return AdminArtworkMappings.ToAdminArtworkDto(updated);
        }

        public async Task<object> RejectArtworkAsync(int artworkId, ArtworkModerationDto dto)
        {
            var artwork = await _repository.GetArtworkByIdAsync(artworkId);
            if (artwork == null) throw new KeyNotFoundException("Artwork not found.");

            var updated = await _repository.UpdateArtworkAsync(artwork);
            return AdminArtworkMappings.ToAdminArtworkDto(updated);
        }

        public async Task<bool> RemoveArtworkAsync(int artworkId)
        {
            return await _repository.RemoveArtworkAsync(artworkId);
        }
    }
}
