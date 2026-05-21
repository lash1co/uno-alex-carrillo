using IssueTracker.Application.DTOs;
using IssueTracker.Application.Validators;
using System.Threading.Tasks;

namespace IssueTracker.Tests.Application.Validators;

public class LoginRequestDtoValidatorTests
{
    private readonly LoginRequestDtoValidator _validator;

    public LoginRequestDtoValidatorTests()
    {
        _validator = new LoginRequestDtoValidator();
    }

    [Fact]
    public async Task Validate_WithValidEmail_ShouldPass()
    {
        var dto = new LoginRequestDto
        {
            Email = "admin@test.com",
            Password = "admin123"
        };

        var result = await _validator.ValidateAsync(dto);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid-email")]
    [InlineData("admin@")]
    public async Task Validate_WithInvalidEmail_ShouldFail(string email)
    {
        var dto = new LoginRequestDto
        {
            Email = email,
            Password = "admin123"
        };

        var result = await _validator.ValidateAsync(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.PropertyName == "Email");
    }
}
