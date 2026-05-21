using AutoMapper;
using IssueTracker.Application.DTOs;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Services;
using IssueTracker.Domain.Entities;
using IssueTracker.Domain.Enums;
using System.Threading.Tasks;
using System;

namespace IssueTracker.Tests.Application.Services;

public class IssueServiceTests
{
    private readonly Mock<IRepository<Issue>> _mockIssueRepository;
    private readonly Mock<IRepository<User>> _mockUserRepository;
    private readonly Mock<IRepository<Attachment>> _mockAttachmentRepository;
    private readonly Mock<IFileStorageService> _mockFileStorageService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IAppDbContext> _mockContext;
    private readonly IssueService _issueService;

    public IssueServiceTests()
    {
        _mockIssueRepository = new Mock<IRepository<Issue>>();
        _mockUserRepository = new Mock<IRepository<User>>();
        _mockAttachmentRepository = new Mock<IRepository<Attachment>>();
        _mockFileStorageService = new Mock<IFileStorageService>();
        _mockMapper = new Mock<IMapper>();
        _mockContext = new Mock<IAppDbContext>();

        _issueService = new IssueService(
            _mockIssueRepository.Object,
            _mockUserRepository.Object,
            _mockAttachmentRepository.Object,
            _mockFileStorageService.Object,
            _mockMapper.Object,
            _mockContext.Object);
    }

    [Fact]
    public async Task CreateIssueAsync_WithValidData_ShouldCreateIssue()
    {
        // Arrange
        var createIssueDto = new CreateIssueDto
        {
            Title = "Test Issue",
            Description = "Test Description",
            Priority = IssuePriority.High
        };

        var expectedIssue = new Issue
        {
            Id = Guid.NewGuid(),
            Title = createIssueDto.Title,
            Description = createIssueDto.Description,
            Priority = createIssueDto.Priority,
            Status = IssueStatus.Open
        };

        var expectedIssueDto = new IssueDto
        {
            Id = expectedIssue.Id,
            Title = expectedIssue.Title,
            Description = expectedIssue.Description,
            Priority = expectedIssue.Priority,
            Status = expectedIssue.Status
        };

        _mockMapper
            .Setup(m => m.Map<Issue>(createIssueDto))
            .Returns(expectedIssue);

        _mockIssueRepository
            .Setup(r => r.AddAsync(It.IsAny<Issue>()))
            .Returns(Task.CompletedTask);

        _mockMapper
            .Setup(m => m.Map<IssueDto>(expectedIssue))
            .Returns(expectedIssueDto);

        // Act
        var result = await _issueService.CreateIssueAsync(createIssueDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expectedIssue.Id);
        result.Title.Should().Be(createIssueDto.Title);
        result.Description.Should().Be(createIssueDto.Description);
        result.Priority.Should().Be(IssuePriority.High);

        _mockIssueRepository.Verify(r => r.AddAsync(It.IsAny<Issue>()), Times.Once);
    }
}
