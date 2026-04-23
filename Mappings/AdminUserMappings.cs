using Artify.Api.DTOs.Admin;
using Artify.Api.Models;

namespace Artify.Api.Mappings
{
    public static class AdminUserMappings
    {
        public static object ToAdminUserDto(ApplicationUser user)
        {
            return new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.RoleType,
                user.IsActive,
                IsApproved = user.ArtistProfile?.IsApproved,
                IsArtist = user.ArtistProfile != null,
                user.CreatedAt
            };
        }

        public static ApplicationUser ApplyStatusUpdate(ApplicationUser user, UpdateUserStatusDto dto)
        {
            user.IsActive = dto.IsActive;
            if (dto.IsApproved.HasValue && user.ArtistProfile != null)
            {
                user.ArtistProfile.IsApproved = dto.IsApproved.Value;
            }
            return user;
        }
    }
}
