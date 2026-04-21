using Artify.Api.DTOs.Auth;
using Artify.Api.Enums;
using Artify.Api.Models;
using Artify.Api.Repositories.Interfaces;
using Artify.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Artify.Api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IArtistRepository _artistRepo;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IConfiguration config,
            IArtistRepository artistRepo)
        {
            _userManager = userManager;
            _config = config;
            _artistRepo = artistRepo;
        }

        // ---------------- REGISTER ----------------

        public async Task<AuthResponseDto> RegisterUserAsync(RegisterDto dto, string role)
        {
            var isAgency = (UserType)dto.UserType == UserType.Agency;

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                UserType = (UserType)dto.UserType, // 0 = Individual, 1 = Agency
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            // Handle Artist Profile Initialization 
            // (Both Artists and Agencies get a profile to showcase work/services)
            if (role == "Artist" || isAgency)
            {
                user.ArtistProfile = new ArtistProfile 
                { 
                    Category = dto.Category,
                    CreatedAt = DateTime.UtcNow,
                    Rating = 0
                };
            }

            // Handle Agency Initialization
            if (isAgency)
            {
                user.OwnedAgency = new Agency
                {
                    Name = $"{dto.FullName}'s Agency",
                    Description = "New Agency Account",
                    TeamSize = dto.TeamSize ?? 0,
                    MemberNames = dto.MemberNames
                };
            }

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            // Role Assignment
            if (isAgency)
            {
                // Agencies get both capabilities
                await _userManager.AddToRoleAsync(user, "Artist");
                await _userManager.AddToRoleAsync(user, "Buyer");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            
            // For token generation, we use the primary role passed from the controller or "Agency" if applicable
            string tokenRole = isAgency ? "Agency" : role;
            return await GenerateTokenAsync(user, tokenRole);
        }

        // ---------------- LOGIN ----------------

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            // Load user with Profile and Agency data included
            var user = await _artistRepo.GetByEmailAsync(dto.Email);
            
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new Exception("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);
            var primaryRole = roles.FirstOrDefault() ?? "Buyer";

            return await GenerateTokenAsync(user, primaryRole);
        }

        // ---------------- JWT GENERATION ----------------

        private async Task<AuthResponseDto> GenerateTokenAsync(ApplicationUser user, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, role),
                new Claim("UserType", ((int)user.UserType).ToString()) // Helpful for Frontend logic
            };

            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new Exception("JWT Key is missing in configuration.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                Role = role,
                FullName = user.FullName,
                Email = user.Email!,
                ProfileImageUrl = user.ArtistProfile?.ProfileImageUrl,
                UserType = (int)user.UserType
            };
        }

        // ---------------- PASSWORD MANAGEMENT ----------------

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return;
            await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) throw new Exception("Invalid user");

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            if (!result.Succeeded) throw new Exception("Password reset failed");
        }
    }
}