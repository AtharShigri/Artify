using Artify.Api.DTOs.Auth;
using Artify.Api.Services.Interfaces;
using Artify.Api.Models; 
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
        private readonly UserManager<Artist> _userManager;
        private readonly IConfiguration _config;

        public AuthService(
            UserManager<Artist> userManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        // ---------------- REGISTER ----------------

        public async Task<AuthResponseDto> RegisterArtistAsync(RegisterDto dto)
            => await RegisterAsync(dto, "Artist");

        public async Task<AuthResponseDto> RegisterBuyerAsync(RegisterDto dto)
            => await RegisterAsync(dto, "Buyer");

        private async Task<AuthResponseDto> RegisterAsync(RegisterDto dto, string role)
        {
            // Create Artist instance instead of IdentityUser
            var user = new Artist
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, role);
            return await GenerateTokenAsync(user, role);
        }

        // ---------------- LOGIN ----------------

        public async Task<AuthResponseDto> LoginArtistAsync(LoginDto dto)
            => await LoginAsync(dto, "Artist");

        public async Task<AuthResponseDto> LoginBuyerAsync(LoginDto dto)
            => await LoginAsync(dto, "Buyer");

        public async Task<AuthResponseDto> LoginAdminAsync(LoginDto dto)
            => await LoginAsync(dto, "Admin");

        private async Task<AuthResponseDto> LoginAsync(LoginDto dto, string role)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Invalid credentials");

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new Exception("Invalid credentials");

            if (!await _userManager.IsInRoleAsync(user, role))
                throw new Exception("Unauthorized role");

            return await GenerateTokenAsync(user, role);
        }

        // ---------------- JWT ----------------

        private async Task<AuthResponseDto> GenerateTokenAsync(Artist user, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, role)
            };

            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey)) throw new Exception("JWT Key is missing in configuration.");

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
                Role = role
            };
        }

        // ---------------- PASSWORD ----------------

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // Email sending intentionally omitted
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Invalid user");

            var result = await _userManager.ResetPasswordAsync(
                user, dto.Token, dto.NewPassword);

            if (!result.Succeeded)
                throw new Exception("Password reset failed");
        }
    }
}