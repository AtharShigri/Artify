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

        public Task<AuthResponseDto> RegisterArtistAsync(RegisterDto dto)
            => RegisterAsync(dto, "Artist");

        public Task<AuthResponseDto> RegisterBuyerAsync(RegisterDto dto)
            => RegisterAsync(dto, "Buyer");

        private async Task<AuthResponseDto> RegisterAsync(RegisterDto dto, string role)
        {
            var user = new Artist
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userManager.CreateAsync(user, dto.Password).ConfigureAwait(false);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, role).ConfigureAwait(false);
            return await GenerateTokenAsync(user, role).ConfigureAwait(false);
        }

        // ---------------- LOGIN ----------------

        public Task<AuthResponseDto> LoginArtistAsync(LoginDto dto)
            => LoginAsync(dto, "Artist");

        public Task<AuthResponseDto> LoginBuyerAsync(LoginDto dto)
            => LoginAsync(dto, "Buyer");

        public Task<AuthResponseDto> LoginAdminAsync(LoginDto dto)
            => LoginAsync(dto, "Admin");

        private async Task<AuthResponseDto> LoginAsync(LoginDto dto, string role)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email).ConfigureAwait(false);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password).ConfigureAwait(false))
                throw new Exception("Invalid credentials");

            if (!await _userManager.IsInRoleAsync(user, role).ConfigureAwait(false))
                throw new Exception("Unauthorized role");

            return await GenerateTokenAsync(user, role).ConfigureAwait(false);
        }

        // ---------------- JWT ----------------

        private Task<AuthResponseDto> GenerateTokenAsync(Artist user, string role)
        {
            var claims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Role, role)
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

            var response = new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                Role = role
            };
            return Task.FromResult(response);
        }

        // ---------------- PASSWORD ----------------

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            if (user == null) return;

            // Token generation is async but not used further here
            await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
            // Email sending intentionally omitted
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email).ConfigureAwait(false);
            if (user == null)
                throw new Exception("Invalid user");

            var result = await _userManager.ResetPasswordAsync(
                user, dto.Token, dto.NewPassword).ConfigureAwait(false);

            if (!result.Succeeded)
                throw new Exception("Password reset failed");
        }
    }
}