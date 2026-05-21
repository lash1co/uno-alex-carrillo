using AutoMapper;
using IssueTracker.Application.DTOs;
using IssueTracker.Application.Interfaces;
using IssueTracker.Domain.Entities;
using IssueTracker.Domain.Enums;
using IssueTracker.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Application.Services;

public class IssueService : IIssueService
{
    private readonly IRepository<Issue> _issueRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Attachment> _attachmentRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IMapper _mapper;
    private readonly IAppDbContext _context;

    public IssueService(
        IRepository<Issue> issueRepository,
        IRepository<User> userRepository,
        IRepository<Attachment> attachmentRepository,
        IFileStorageService fileStorageService,
        IMapper mapper,
        IAppDbContext context)
    {
        _issueRepository = issueRepository;
        _userRepository = userRepository;
        _attachmentRepository = attachmentRepository;
        _fileStorageService = fileStorageService;
        _mapper = mapper;
        _context = context;
    }

    public async Task<PaginatedResponseDto<IssueDto>> GetIssuesAsync(
        int page = 1,
        int pageSize = 10,
        IssueStatus? status = null)
    {
        // Validate pagination parameters
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        // Query with eager loading to prevent N+1
        var query = _context.Issues
            .Include(i => i.Assignee)
            .Include(i => i.Attachments)
            .AsQueryable();

        // Apply filtering
        if (status.HasValue)
        {
            query = query.Where(i => i.Status == status.Value);
        }

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var issues = await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var issueDtos = _mapper.Map<List<IssueDto>>(issues);

        return new PaginatedResponseDto<IssueDto>
        {
            Items = issueDtos,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<IssueDto> GetIssueByIdAsync(Guid id)
    {
        // Eager loading to prevent N+1
        var issue = await _context.Issues
            .Include(i => i.Assignee)
            .Include(i => i.Attachments)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (issue == null)
        {
            throw new NotFoundException(nameof(Issue), id);
        }

        return _mapper.Map<IssueDto>(issue);
    }

    public async Task<IssueDto> CreateIssueAsync(CreateIssueDto createIssueDto)
    {
        var issue = _mapper.Map<Issue>(createIssueDto);

        await _issueRepository.AddAsync(issue);

        return _mapper.Map<IssueDto>(issue);
    }

    public async Task<IssueDto> UpdateIssueAsync(Guid id, UpdateIssueDto updateIssueDto)
    {
        // Eager loading
        var issue = await _context.Issues
            .Include(i => i.Assignee)
            .Include(i => i.Attachments)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (issue == null)
        {
            throw new NotFoundException(nameof(Issue), id);
        }

        // Validate assignee exists if provided
        if (updateIssueDto.AssigneeId.HasValue)
        {
            var user = await _userRepository.GetByIdAsync(updateIssueDto.AssigneeId.Value);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), updateIssueDto.AssigneeId.Value);
            }
        }

        // Update properties only if provided
        if (updateIssueDto.Status.HasValue)
        {
            issue.Status = updateIssueDto.Status.Value;
        }

        if (updateIssueDto.Priority.HasValue)
        {
            issue.Priority = updateIssueDto.Priority.Value;
        }

        if (updateIssueDto.AssigneeId.HasValue)
        {
            issue.AssigneeId = updateIssueDto.AssigneeId.Value;
        }

        issue.UpdatedAt = DateTime.UtcNow;

        await _issueRepository.UpdateAsync(issue);

        return _mapper.Map<IssueDto>(issue);
    }

    public async Task DeleteIssueAsync(Guid id)
    {
        var issue = await _context.Issues
            .Include(i => i.Attachments)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (issue == null)
        {
            throw new NotFoundException(nameof(Issue), id);
        }

        foreach (var attachment in issue.Attachments)
        {
            DeleteAttachmentFile(attachment.FilePath);
        }

        DeleteIssueUploadsDirectory(id);

        await _issueRepository.DeleteAsync(issue);
    }

    public async Task<AttachmentDto> UploadAttachmentAsync(Guid issueId, IFormFile? file)
    {
        //verify file is not null and has content
        if (file == null || file.Length == 0)
        {
            throw new BadRequestException(
                "File is required.");
        }

        // Verify issue exists
        var issue = await _issueRepository.GetByIdAsync(issueId);
        if (issue == null)
        {
            throw new NotFoundException(nameof(Issue), issueId);
        }

        // Save file using storage service
        var filePath = await _fileStorageService.SaveFileAsync(file, issueId);

        // Create attachment entity
        var attachment = new Attachment
        {
            Id = Guid.NewGuid(),
            FileName = file.FileName,
            FilePath = filePath,
            IssueId = issueId,
            UploadedAt = DateTime.UtcNow
        };

        await _attachmentRepository.AddAsync(attachment);

        return _mapper.Map<AttachmentDto>(attachment);
    }

    public async Task DeleteAttachmentAsync(Guid issueId, Guid attachmentId)
    {
        // Verify issue exists
        var issue = await _issueRepository.GetByIdAsync(issueId);
        if (issue == null)
        {
            throw new NotFoundException(nameof(Issue), issueId);
        }

        // Get attachment
        var attachment = await _context.Attachments
            .FirstOrDefaultAsync(a => a.Id == attachmentId && a.IssueId == issueId);

        if (attachment == null)
        {
            throw new NotFoundException(nameof(Attachment), attachmentId);
        }

        DeleteAttachmentFile(attachment.FilePath);

        await _attachmentRepository.DeleteAsync(attachment);
    }

    private static void DeleteAttachmentFile(string filePath)
    {
        if (!string.IsNullOrWhiteSpace(filePath) &&
            File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    private static void DeleteIssueUploadsDirectory(Guid issueId)
    {
        var uploadsPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "uploads",
            issueId.ToString());

        if (Directory.Exists(uploadsPath))
        {
            Directory.Delete(uploadsPath, recursive: true);
        }
    }
}
