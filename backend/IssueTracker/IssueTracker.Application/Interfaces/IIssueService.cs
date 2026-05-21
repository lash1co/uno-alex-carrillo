using IssueTracker.Application.DTOs;
using IssueTracker.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace IssueTracker.Application.Interfaces;

public interface IIssueService
{
    Task<PaginatedResponseDto<IssueDto>> GetIssuesAsync(
        int page = 1,
        int pageSize = 10,
        IssueStatus? status = null);

    Task<IssueDto> GetIssueByIdAsync(Guid id);

    Task<IssueDto> CreateIssueAsync(CreateIssueDto createIssueDto);

    Task<IssueDto> UpdateIssueAsync(Guid id, UpdateIssueDto updateIssueDto);

    Task DeleteIssueAsync(Guid id);

    Task<AttachmentDto> UploadAttachmentAsync(Guid issueId, IFormFile? file);

    Task DeleteAttachmentAsync(Guid issueId, Guid attachmentId);
}
