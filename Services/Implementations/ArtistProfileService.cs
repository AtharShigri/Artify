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
        private readonly UserManager<Artist> _userManager;
        private readonly SignInManager<Artist> _signInManager;
        private readonly IConfiguration _config;

        public ArtistProfileService(
            IArtistRepository artistRepo,
            UserManager<Artist> userManager,
            SignInManager<Artist> signInManager,
            IConfiguration config)
        {
            _artistRepo = artistRepo;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        // REGISTER
        public async Task<object> RegisterAsync(ArtistRegisterDto dto)
        {
            var artist = new Artist
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email,
                Category = dto.Category
            };

            var result = await _userManager.CreateAsync(artist, dto.Password);
            if (!result.Succeeded)
                return new { Success = false, Errors = result.Errors.Select(e => e.Description) };

            // Assign Role
            var roleResult = await _userManager.AddToRoleAsync(artist, "Artist");
            if (!roleResult.Succeeded)
                return new { Success = false, Errors = roleResult.Errors.Select(e => e.Description) };

            return new { Success = true, Message = "Artist registered successfully" };
        }

        // LOGIN
        public async Task<object> LoginAsync(LoginDto dto)
        {
            var artist = await _userManager.FindByEmailAsync(dto.Email);
            if (artist == null)
                return new { Success = false, Message = "Invalid credentials" };

            var result = await _signInManager.CheckPasswordSignInAsync(artist, dto.Password, false);
            if (!result.Succeeded)
                return new { Success = false, Message = "Invalid credentials" };

            // Verify Role (Security Check & Self-Healing for legacy users)
            if (!await _userManager.IsInRoleAsync(artist, "Artist"))
            {
                // Attempt to auto-correct by assigning role if they are successfully authenticated as an Artist entity
                var addRoleResult = await _userManager.AddToRoleAsync(artist, "Artist");
                if (!addRoleResult.Succeeded)
                {
                     return new { Success = false, Message = "Unauthorized: User is not an Artist and role assignment failed." };
                }
            }

            // CLAIMS
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, artist.Id.ToString()),
                new Claim(ClaimTypes.Email, artist.Email!),
                new Claim(ClaimTypes.Role, "Artist")
            };

            // JWT KEY
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new
            {
                Success = true,
                token = jwt,
                role = "artist",
                ArtistId = artist.Id,
                Email = artist.Email
            };
        }

        // GET PROFILE
        public async Task<object> GetProfileAsync(ClaimsPrincipal user)
        {
            var artistId = _userManager.GetUserId(user);
            var artist = await _userManager.GetUserAsync(user);
            if (artist == null) return null;

            return new
            {
                artist.Id,
                artist.FullName,
                artist.Email,
                artist.Bio,
                artist.Category,
                artist.Phone,
                artist.City,
                artist.SocialLink,
                artist.ProfileImageUrl
            };
        }

        // UPDATE PROFILE
        public async Task<object> UpdateProfileAsync(ClaimsPrincipal user, ArtistUpdateDto dto)
        {
            var artistId = _userManager.GetUserId(user);
            var artist = await _userManager.GetUserAsync(user);
            if (artist == null) return null;

            artist.FullName = dto.FullName ?? artist.FullName;
            artist.Bio = dto.Bio ?? artist.Bio;
            artist.Category = dto.Category ?? artist.Category;
            artist.Phone = dto.Phone ?? artist.Phone;
            artist.City = dto.City ?? artist.City;
            artist.SocialLink = dto.SocialLink ?? artist.SocialLink;

            await _userManager.UpdateAsync(artist);

            return new { Success = true, Message = "Profile updated successfully" };
        }

        // UPDATE PROFILE IMAGE
        public async Task<object> UpdateProfileImageAsync(ClaimsPrincipal user, IFormFile file)
        {
            var artistId = _userManager.GetUserId(user);
            var artist = await _userManager.GetUserAsync(user);
            if (artist == null) return null;

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var path = Path.Combine("wwwroot/images/artists", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!); 
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            artist.ProfileImageUrl = $"/images/artists/{fileName}";
            await _artistRepo.UpdateAsync(artist);

            return new { Success = true, ProfileImageUrl = artist.ProfileImageUrl };
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
