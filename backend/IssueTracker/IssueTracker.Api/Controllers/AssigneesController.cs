using IssueTracker.Application.DTOs;
using IssueTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AssigneesController : ControllerBase
{
    private readonly IAssigneeService _assigneeService;

    public AssigneesController(IAssigneeService assigneeService)
    {
        _assigneeService = assigneeService;
    }

    /// <summary>
    /// Gets the available users that can be assigned to issues
    /// </summary>
    /// <returns>List of assignees with ID and name</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AssigneeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AssigneeDto>>> GetAssignees()
    {
        var result = await _assigneeService.GetAssigneesAsync();

        return Ok(result);
    }
}
