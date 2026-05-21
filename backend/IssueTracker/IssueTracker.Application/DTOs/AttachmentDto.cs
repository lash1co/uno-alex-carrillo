namespace IssueTracker.Application.DTOs;

public class AttachmentDto
{
    public Guid Id { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FileUrl { get; set; } = string.Empty;

    public DateTime UploadedAt { get; set; }
}
