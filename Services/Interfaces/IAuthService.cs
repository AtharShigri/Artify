using artifi.Api.DTOs.Auth;

namespace artifi.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterUserAsync(RegisterDto dto, string role);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);

        Task ForgotPasswordAsync(string email);
        Task ResetPasswordAsync(ResetPasswordDto dto);
    }
}