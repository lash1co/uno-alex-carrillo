using IssueTracker.Application.DTOs;
using IssueTracker.Application.Validators;
using IssueTracker.Domain.Enums;
using System.Threading.Tasks;

namespace IssueTracker.Tests.Application.Validators;

public class CreateIssueDtoValidatorTests
{
    private readonly CreateIssueDtoValidator _validator;

    public CreateIssueDtoValidatorTests()
    {
        _validator = new CreateIssueDtoValidator();
    }

    [Fact]
    public async Task Validate_WithValidData_ShouldPass()
    {
        // Arrange
        var dto = new CreateIssueDto
        {
            Title = "Valid Title",
            Description = "Valid Description",
            Priority = IssuePriority.High
        };

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Validate_WithEmptyTitle_ShouldFail()
    {
        // Arrange
        var dto = new CreateIssueDto
        {
            Title = "",
            Description = "Valid Description",
            Priority = IssuePriority.Low
        };

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public async Task Validate_WithTitleExceedingMaxLength_ShouldFail()
    {
        // Arrange
        var dto = new CreateIssueDto
        {
            Title = new string('a', 201),
            Description = "Valid Description",
            Priority = IssuePriority.Low
        };

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public async Task Validate_WithEmptyDescription_ShouldFail()
    {
        // Arrange
        var dto = new CreateIssueDto
        {
            Title = "Valid Title",
            Description = "",
            Priority = IssuePriority.Low
        };

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Description");
    }

    [Fact]
    public async Task Validate_WithDescriptionExceedingMaxLength_ShouldFail()
    {
        // Arrange
        var dto = new CreateIssueDto
        {
            Title = "Valid Title",
            Description = new string('a', 2001),
            Priority = IssuePriority.Low
        };

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Description");
    }

    [Theory]
    [InlineData(IssuePriority.Low)]
    [InlineData(IssuePriority.Medium)]
    [InlineData(IssuePriority.High)]
    public async Task Validate_WithValidPriority_ShouldPass(IssuePriority priority)
    {
        // Arrange
        var dto = new CreateIssueDto
        {
            Title = "Valid Title",
            Description = "Valid Description",
            Priority = priority
        };

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithMultipleErrors_ShouldReturnAllErrors()
    {
        // Arrange
        var dto = new CreateIssueDto
        {
            Title = "",
            Description = "",
            Priority = IssuePriority.Low
        };

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThanOrEqualTo(2);
    }
}
