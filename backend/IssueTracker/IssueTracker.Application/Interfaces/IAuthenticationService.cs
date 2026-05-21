using IssueTracker.Application.DTOs;

namespace IssueTracker.Application.Interfaces;

public interface IAuthenticationService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);

    string GenerateJwtToken(string email);
}
