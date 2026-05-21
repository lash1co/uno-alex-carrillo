using IssueTracker.Domain.Entities;
using IssueTracker.Domain.Enums;
using Xunit;
using FluentAssertions;
using System;
using System.Linq;

namespace IssueTracker.Tests.Domain.Entities;

public class IssueTests
{
    [Fact]
    public void Issue_ShouldHaveDefaultValues_WhenCreated()
    {
        // Arrange & Act
        var issue = new Issue();

        // Assert
        issue.Id.Should().Be(Guid.Empty);
        issue.Title.Should().Be(string.Empty);
        issue.Description.Should().Be(string.Empty);
        issue.Status.Should().Be(IssueStatus.Open);
        issue.Priority.Should().Be(IssuePriority.Low);
        issue.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        issue.UpdatedAt.Should().BeNull();
        issue.AssigneeId.Should().BeNull();
        issue.Assignee.Should().BeNull();
        issue.Attachments.Should().BeEmpty();
    }

    [Fact]
    public void Issue_ShouldInitializeAttachmentsAsEmptyList_WhenCreated()
    {
        // Arrange & Act
        var issue = new Issue();

        // Assert
        issue.Attachments.Should().NotBeNull();
        issue.Attachments.Should().BeEmpty();
    }

    [Fact]
    public void Issue_ShouldAllowSettingProperties()
    {
        // Arrange
        var issue = new Issue();
        var issueId = Guid.NewGuid();
        var title = "Critical Bug";
        var description = "The application crashes when...";
        var status = IssueStatus.InProgress;
        var priority = IssuePriority.High;

        // Act
        issue.Id = issueId;
        issue.Title = title;
        issue.Description = description;
        issue.Status = status;
        issue.Priority = priority;

        // Assert
        issue.Id.Should().Be(issueId);
        issue.Title.Should().Be(title);
        issue.Description.Should().Be(description);
        issue.Status.Should().Be(status);
        issue.Priority.Should().Be(priority);
    }

    [Fact]
    public void Issue_ShouldAllowAssigningToUser()
    {
        // Arrange
        var issue = new Issue();
        var assignee = new User { Id = Guid.NewGuid(), Name = "John Doe", Email = "john@example.com" };
        var assigneeId = assignee.Id;

        // Act
        issue.AssigneeId = assigneeId;
        issue.Assignee = assignee;

        // Assert
        issue.AssigneeId.Should().Be(assigneeId);
        issue.Assignee.Should().Be(assignee);
        issue.Assignee?.Name.Should().Be("John Doe");
    }

    [Fact]
    public void Issue_ShouldAllowAddingAttachments()
    {
        // Arrange
        var issue = new Issue();
        var attachment = new Attachment
        {
            Id = Guid.NewGuid(),
            FileName = "document.pdf",
            FilePath = "/uploads/document.pdf",
            IssueId = issue.Id
        };

        // Act
        issue.Attachments.Add(attachment);

        // Assert
        issue.Attachments.Should().HaveCount(1);
        issue.Attachments.First().FileName.Should().Be("document.pdf");
    }

    [Theory]
    [InlineData(IssueStatus.Open)]
    [InlineData(IssueStatus.InProgress)]
    [InlineData(IssueStatus.Closed)]
    public void Issue_ShouldSupportAllStatusValues(IssueStatus status)
    {
        // Arrange & Act
        var issue = new Issue { Status = status };

        // Assert
        issue.Status.Should().Be(status);
    }

    [Theory]
    [InlineData(IssuePriority.Low)]
    [InlineData(IssuePriority.Medium)]
    [InlineData(IssuePriority.High)]
    public void Issue_ShouldSupportAllPriorityValues(IssuePriority priority)
    {
        // Arrange & Act
        var issue = new Issue { Priority = priority };

        // Assert
        issue.Priority.Should().Be(priority);
    }
}
