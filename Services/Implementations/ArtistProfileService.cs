using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Artify.Api.DTOs.Artist;
using Artify.Api.DTOs.Auth;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Artify.Api.Services.Implementations
{
    public class ArtistProfileService : IArtistProfileService
    {
        private readonly IArtistRepository _artistRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment;

        public ArtistProfileService(
            IArtistRepository artistRepo,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config,
            IWebHostEnvironment environment)
        {
            _environment = environment;
            _artistRepo = artistRepo;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }


        // GET PROFILE
        public async Task<object> GetProfileAsync(ClaimsPrincipal user)
        {
            var artistId = _userManager.GetUserId(user);
            var guidId = Guid.Parse(artistId);
            var artist = await _artistRepo.GetByIdAsync(guidId);
            if (artist == null) return null;

            return new
            {
                artist.Id,
                artist.FullName,
                artist.Email,
                Bio = artist.ArtistProfile?.Bio,
                Category = artist.ArtistProfile?.Category,
                Phone = artist.PhoneNumber,
                City = artist.ArtistProfile?.Location,
                SocialLink = artist.ArtistProfile?.SocialLinks,
                ProfileImageUrl = artist.ArtistProfile?.ProfileImageUrl
            };
        }

        // UPDATE PROFILE
        public async Task<object> UpdateProfileAsync(ClaimsPrincipal user, ArtistUpdateDto dto)
        {
            var artistId = _userManager.GetUserId(user);
            var guidId = Guid.Parse(artistId);
            var artist = await _artistRepo.GetByIdAsync(guidId);
            if (artist == null) return null;

            artist.FullName = dto.FullName ?? artist.FullName;
            if (dto.Phone != null) artist.PhoneNumber = dto.Phone;
            
            if (artist.ArtistProfile == null)
            {
                artist.ArtistProfile = new ArtistProfile();
            }

            artist.ArtistProfile.Bio = dto.Bio ?? artist.ArtistProfile.Bio;
            artist.ArtistProfile.Category = dto.Category ?? artist.ArtistProfile.Category;
            artist.ArtistProfile.Location = dto.City ?? artist.ArtistProfile.Location;
            artist.ArtistProfile.SocialLinks = dto.SocialLink ?? artist.ArtistProfile.SocialLinks;

            await _artistRepo.UpdateAsync(artist);

            return new { Success = true, Message = "Profile updated successfully" };
        }

        // UPDATE PROFILE IMAGE

public async Task<object> UpdateProfileImageAsync(ClaimsPrincipal user, IFormFile Image)
{
    var artistId = _userManager.GetUserId(user);
    if (string.IsNullOrEmpty(artistId)) return null; 
    var artist = await _artistRepo.GetByIdAsync(Guid.Parse(artistId));
    if (artist == null) return null;

    var fileName = $"{Guid.NewGuid()}_{Image.FileName}";
    
    // Use ContentRootPath or WebRootPath to ensure it hits the physical disk correctly
    var folderPath = Path.Combine(_environment.ContentRootPath, "wwwroot", "images", "artists");
    var filePath = Path.Combine(folderPath, fileName);

    Directory.CreateDirectory(folderPath); 

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await Image.CopyToAsync(stream);
    }

    if (artist.ArtistProfile == null) artist.ArtistProfile = new ArtistProfile();

    // Store the relative URL for the frontend
    artist.ArtistProfile.ProfileImageUrl = $"/images/artists/{fileName}";
    await _artistRepo.UpdateAsync(artist);

    return new { Success = true, ProfileImageUrl = artist.ArtistProfile.ProfileImageUrl };
}

        // DELETE PROFILE
        public async Task<object> DeleteProfileAsync(ClaimsPrincipal user)
        {
            var artistId = _userManager.GetUserId(user);
            var artist = await _userManager.GetUserAsync(user);
            if (artist == null) return null;

            await _userManager.DeleteAsync(artist);

            return new { Success = true, Message = "Profile deleted successfully" };
        }
    }
}
