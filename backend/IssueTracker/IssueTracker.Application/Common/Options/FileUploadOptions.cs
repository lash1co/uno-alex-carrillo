namespace IssueTracker.Application.Common.Options;

public class FileUploadOptions
{
    public int MaxFileSizeMB { get; set; }

    public string[] AllowedExtensions { get; set; }
        = Array.Empty<string>();
}
