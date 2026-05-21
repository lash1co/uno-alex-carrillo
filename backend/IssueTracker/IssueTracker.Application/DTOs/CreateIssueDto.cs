using IssueTracker.Domain.Enums;

namespace IssueTracker.Application.DTOs;

public class CreateIssueDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public IssuePriority Priority { get; set; } = IssuePriority.Low;
}
