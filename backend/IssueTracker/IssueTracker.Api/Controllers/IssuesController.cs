using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IssueTracker.Application.DTOs;
using IssueTracker.Application.Interfaces;
using IssueTracker.Domain.Enums;

namespace IssueTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class IssuesController : ControllerBase
{
    private readonly IIssueService _issueService;

    public IssuesController(
        IIssueService issueService)
    {
        _issueService = issueService;
    }
    /// <summary>
    /// Gets a paginated list of issues with optional status filtering
    /// </summary>
    /// <param name="page">Page number (default 1)</param>
    /// <param name="pageSize">Number of items per page (default 10, max 100)</param>
    /// <param name="status">Filter by status: Open, InProgress, or Closed (optional)</param>
    /// <returns>Paginated list of issues</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponseDto<IssueDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedResponseDto<IssueDto>>> GetIssues(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] IssueStatus? status = null)
    {
        var result = await _issueService.GetIssuesAsync(page, pageSize, status);
        return Ok(result);
    }

    /// <summary>
    /// Gets a single issue by ID
    /// </summary>
    /// <param name="id">Issue ID</param>
    /// <returns>Issue details</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(IssueDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IssueDto>> GetIssue(Guid id)
    {
        var result = await _issueService.GetIssueByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new issue
    /// </summary>
    /// <param name="createIssueDto">Issue creation details</param>
    /// <returns>Created issue</returns>
    [HttpPost]
    [ProducesResponseType(typeof(IssueDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IssueDto>> CreateIssue(CreateIssueDto createIssueDto)
    {
        var result = await _issueService.CreateIssueAsync(createIssueDto);
        return CreatedAtAction(nameof(GetIssue), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing issue
    /// </summary>
    /// <param name="id">Issue ID</param>
    /// <param name="updateIssueDto">Update details (optional fields)</param>
    /// <returns>Updated issue</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(IssueDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IssueDto>> UpdateIssue(Guid id, UpdateIssueDto updateIssueDto)
    {
        var result = await _issueService.UpdateIssueAsync(id, updateIssueDto);
        return Ok(result);
    }

    /// <summary>
    /// Deletes an issue
    /// </summary>
    /// <param name="id">Issue ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteIssue(Guid id)
    {
        await _issueService.DeleteIssueAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Uploads an attachment to an issue (max 5MB, images only)
    /// </summary>
    /// <param name="issueId">Issue ID</param>
    /// <param name="file">File to upload (jpg, jpeg, png)</param>
    /// <returns>Attachment details</returns>
    [HttpPost("{issueId:guid}/attachments")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(AttachmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AttachmentDto>> UploadAttachment(
        Guid issueId,
        IFormFile? file)
    {
        var result = await _issueService.UploadAttachmentAsync(issueId, file);
        return Created(result.FileUrl, result);
    }

    /// <summary>
    /// Deletes an attachment from an issue
    /// </summary>
    /// <param name="issueId">Issue ID</param>
    /// <param name="fileId">Attachment ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{issueId:guid}/attachments/{fileId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAttachment(Guid issueId, Guid fileId)
    {
        await _issueService.DeleteAttachmentAsync(issueId, fileId);
        return NoContent();
    }
}
