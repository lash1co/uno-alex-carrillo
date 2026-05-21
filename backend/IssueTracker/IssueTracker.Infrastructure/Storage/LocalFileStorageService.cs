using Microsoft.AspNetCore.Http;
using IssueTracker.Domain.Exceptions;
using IssueTracker.Application.Interfaces;
using Microsoft.Extensions.Options;
using IssueTracker.Application.Common.Options;

namespace IssueTracker.Infrastructure.Storage;

public class LocalFileStorageService
    : IFileStorageService
{
    private readonly FileUploadOptions _options;

    public LocalFileStorageService(
        IOptions<FileUploadOptions> options)
    {
        _options = options.Value;
    }

    public async Task<string> SaveFileAsync(
        IFormFile file,
        Guid issueId)
    {
        var maxFileSize = _options.MaxFileSizeMB * 1024 * 1024;

        if (file.Length > maxFileSize)
        {
            throw new BadRequestException(
                "File size cannot exceed 5 MB.");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!_options.AllowedExtensions.Contains(extension))
        {
            throw new BadRequestException(
                "Invalid file type.");
        }

        var uploadsFolder = Path.Combine(
            Directory.GetCurrentDirectory(),
            "uploads",
            issueId.ToString());

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var fileName =
            $"{Guid.NewGuid()}_{file.FileName}";

        var filePath = Path.Combine(
            uploadsFolder,
            fileName);

        await using var stream =
            new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(stream);

        return filePath;
    }
}
