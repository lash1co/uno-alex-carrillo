using IssueTracker.Application.DTOs;

namespace IssueTracker.Application.Interfaces;

public interface IAssigneeService
{
    Task<IEnumerable<AssigneeDto>> GetAssigneesAsync();
}
