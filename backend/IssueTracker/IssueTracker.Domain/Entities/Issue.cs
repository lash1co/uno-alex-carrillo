using IssueTracker.Domain.Enums;

namespace IssueTracker.Domain.Entities;

public class Issue
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public IssueStatus Status { get; set; } = IssueStatus.Open;

    public IssuePriority Priority { get; set; } = IssuePriority.Low;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public Guid? AssigneeId { get; set; }

    public User? Assignee { get; set; }

    public List<Attachment> Attachments { get; set; } = [];
}