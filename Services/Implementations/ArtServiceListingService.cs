using System.Security.Claims;
using Artify.Api.DTOs.Artist;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;

namespace Artify.Api.Services.Implementations
{
    public class ArtServiceListingService : IArtServiceListingService
    {
        private readonly IArtServiceRepository _serviceRepo;
        private readonly IArtistRepository _artistRepo;

        public ArtServiceListingService(
            IArtServiceRepository serviceRepo,
            IArtistRepository artistRepo)
        {
            _serviceRepo = serviceRepo;
            _artistRepo = artistRepo;
        }

        public async Task<object> GetAllAsync(ClaimsPrincipal user)
        {
            var artistId = _artistRepo.GetArtistId(user);
            var services = await _serviceRepo.GetAllByArtistAsync(artistId);

            return services.Select(s => new
            {
                s.Id,
                s.Title,
                s.Description,
                s.Price,
                s.Duration,
                s.Category
            });
        }

        public async Task<object> GetByIdAsync(ClaimsPrincipal user, int serviceId)
        {
            var artistId = _artistRepo.GetArtistId(user);
            var service = await _serviceRepo.GetByIdAsync(serviceId);

            if (service == null || service.ArtistId != artistId)
                return null;

            return new
            {
                service.Id,
                service.Title,
                service.Description,
                service.Price,
                service.Duration,
                service.Category
            };
        }

        public async Task<object> CreateAsync(ClaimsPrincipal user, ArtServiceDto dto)
        {
            var artistId = _artistRepo.GetArtistId(user);

            var service = new ArtService
            {
                ArtistId = artistId,
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                Duration = dto.Duration,
                Category = dto.Category
            };

            await _serviceRepo.AddAsync(service);
            return new { Success = true, ServiceId = service.Id };
        }

        public async Task<object> UpdateAsync(ClaimsPrincipal user, int serviceId, ArtServiceDto dto)
        {
            var artistId = _artistRepo.GetArtistId(user);
            var service = await _serviceRepo.GetByIdAsync(serviceId);

            if (service == null || service.ArtistId != artistId)
                return null;

            service.Title = dto.Title ?? service.Title;
            service.Description = dto.Description ?? service.Description;
            service.Price = dto.Price;
            service.Duration = dto.Duration ?? service.Duration;
            service.Category = dto.Category ?? service.Category;

            await _serviceRepo.UpdateAsync(service);
            return new { Success = true };
        }

        public async Task<object> DeleteAsync(ClaimsPrincipal user, int serviceId)
        {
            var artistId = _artistRepo.GetArtistId(user);
            var service = await _serviceRepo.GetByIdAsync(serviceId);

            if (service == null || service.ArtistId != artistId)
                return null;

            await _serviceRepo.DeleteAsync(service);
            return new { Success = true };
        }
    }
}
