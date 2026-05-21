using FluentValidation;
using IssueTracker.Application.DTOs;

namespace IssueTracker.Application.Validators;

public class CreateIssueDtoValidator : AbstractValidator<CreateIssueDto>
{
    public CreateIssueDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(200)
            .WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(2000)
            .WithMessage("Description cannot exceed 2000 characters");

        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Priority must be a valid value (Low, Medium, High)");
    }
}
