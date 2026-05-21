using FluentValidation;
using IssueTracker.Application.DTOs;

namespace IssueTracker.Application.Validators;

public class UpdateIssueDtoValidator : AbstractValidator<UpdateIssueDto>
{
    public UpdateIssueDtoValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Status must be a valid value (Open, InProgress, Closed)")
            .When(x => x.Status.HasValue);

        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Priority must be a valid value (Low, Medium, High)")
            .When(x => x.Priority.HasValue);

        RuleFor(x => x.AssigneeId)
            .NotEmpty()
            .WithMessage("AssigneeId must be a valid GUID")
            .When(x => x.AssigneeId.HasValue);
    }
}
