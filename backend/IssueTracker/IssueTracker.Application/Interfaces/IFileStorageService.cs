using Microsoft.AspNetCore.Http;

namespace IssueTracker.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, Guid issueId);
}