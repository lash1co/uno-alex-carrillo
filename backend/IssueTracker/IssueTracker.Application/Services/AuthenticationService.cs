using FluentValidation;
using IssueTracker.Application.DTOs;
using IssueTracker.Application.Interfaces;
using IssueTracker.Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IssueTracker.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _configuration;
    private readonly IValidator<LoginRequestDto> _loginValidator;

    public AuthenticationService(
        IConfiguration configuration,
        IValidator<LoginRequestDto> loginValidator)
    {
        _configuration = configuration;
        _loginValidator = loginValidator;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var validationResult = await _loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(
                " ",
                validationResult.Errors.Select(error => error.ErrorMessage));

            throw new BadRequestException(errors);
        }

        var testUserEmail = _configuration["TestUser:Email"];
        var testUserPassword = _configuration["TestUser:Password"];

        if (request.Email != testUserEmail || request.Password != testUserPassword)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var token = GenerateJwtToken(request.Email);
        var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");

        return await Task.FromResult(new LoginResponseDto
        {
            Token = token,
            User = new UserDto
            {
                Id = Guid.Empty,
                Name = "System User",
                Email = testUserEmail
            },
            ExpirationMinutes = expirationMinutes
        });
    }

    public string GenerateJwtToken(string email)
    {
        var jwtKey = _configuration["Jwt:Key"];
        var key = Encoding.UTF8.GetBytes(jwtKey!);
        var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("role", "api_user")
            }),
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
