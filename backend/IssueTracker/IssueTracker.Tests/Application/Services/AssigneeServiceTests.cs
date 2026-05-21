using IssueTracker.Application.Services;
using IssueTracker.Application.Interfaces;
using IssueTracker.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace IssueTracker.Tests.Application.Services;

public class AssigneeServiceTests
{
    private readonly Mock<IRepository<User>> _mockUserRepository;
    private readonly AssigneeService _assigneeService;

    public AssigneeServiceTests()
    {
        _mockUserRepository = new Mock<IRepository<User>>();
        _assigneeService = new AssigneeService(_mockUserRepository.Object);
    }

    [Fact]
    public async Task GetAssigneesAsync_ShouldReturnOnlyIdAndNameOrderedByName()
    {
        var users = new List<User>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Zoe",
                Email = "zoe@example.com"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Ana",
                Email = "ana@example.com"
            }
        };

        _mockUserRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(users);

        var result = (await _assigneeService.GetAssigneesAsync()).ToList();

        result.Should().HaveCount(2);
        result[0].Id.Should().Be(users[1].Id);
        result[0].Name.Should().Be("Ana");
        result[1].Id.Should().Be(users[0].Id);
        result[1].Name.Should().Be("Zoe");
    }
}
