using IssueTracker.Application.DTOs;
using IssueTracker.Application.Interfaces;
using IssueTracker.Domain.Entities;

namespace IssueTracker.Application.Services;

public class AssigneeService : IAssigneeService
{
    private readonly IRepository<User> _userRepository;

    public AssigneeService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<AssigneeDto>> GetAssigneesAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users
            .OrderBy(user => user.Name)
            .Select(user => new AssigneeDto
            {
                Id = user.Id,
                Name = user.Name
            });
    }
}
