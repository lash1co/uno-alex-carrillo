namespace IssueTracker.Domain.Entities;

public class Attachment
{
    public Guid Id { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public Guid IssueId { get; set; }

    public Issue Issue { get; set; } = null!;
}
