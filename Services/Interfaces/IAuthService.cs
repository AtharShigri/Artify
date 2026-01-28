using Artify.Api.DTOs.Auth;

namespace Artify.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterArtistAsync(RegisterDto dto);
        Task<AuthResponseDto> RegisterBuyerAsync(RegisterDto dto);

        Task<AuthResponseDto> LoginArtistAsync(LoginDto dto);
        Task<AuthResponseDto> LoginBuyerAsync(LoginDto dto);
        Task<AuthResponseDto> LoginAdminAsync(LoginDto dto);

        Task ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(ResetPasswordDto dto);
    }
}
