using IssueTracker.Domain.Enums;

namespace IssueTracker.Application.DTOs;

public class UpdateIssueDto
{
    public IssueStatus? Status { get; set; }

    public IssuePriority? Priority { get; set; }

    public Guid? AssigneeId { get; set; }
}
