using IssueTracker.Domain.Entities;
using Xunit;
using FluentAssertions;
using System;
using System.Linq;
using System.Collections.Generic;

namespace IssueTracker.Tests.Domain.Entities;

public class UserTests
{
    [Fact]
    public void User_ShouldHaveDefaultValues_WhenCreated()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        user.Id.Should().Be(Guid.Empty);
        user.Name.Should().Be(string.Empty);
        user.Email.Should().Be(string.Empty);
        user.AssignedIssues.Should().BeEmpty();
    }

    [Fact]
    public void User_ShouldInitializeAssignedIssuesAsEmptyList_WhenCreated()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        user.AssignedIssues.Should().NotBeNull();
        user.AssignedIssues.Should().BeEmpty();
    }

    [Fact]
    public void User_ShouldAllowSettingProperties()
    {
        // Arrange
        var user = new User();
        var userId = Guid.NewGuid();
        var name = "Jane Doe";
        var email = "jane@example.com";

        // Act
        user.Id = userId;
        user.Name = name;
        user.Email = email;

        // Assert
        user.Id.Should().Be(userId);
        user.Name.Should().Be(name);
        user.Email.Should().Be(email);
    }

    [Fact]
    public void User_ShouldAllowAddingAssignedIssues()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), Name = "John", Email = "john@example.com" };
        var issue = new Issue
        {
            Id = Guid.NewGuid(),
            Title = "Test Issue",
            AssigneeId = user.Id
        };

        // Act
        user.AssignedIssues.Add(issue);

        // Assert
        user.AssignedIssues.Should().HaveCount(1);
        user.AssignedIssues.First().Title.Should().Be("Test Issue");
    }

    [Fact]
    public void User_ShouldAllowAddingMultipleAssignedIssues()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), Name = "John", Email = "john@example.com" };
        var issues = new List<Issue>
        {
            new() { Id = Guid.NewGuid(), Title = "Issue 1", AssigneeId = user.Id },
            new() { Id = Guid.NewGuid(), Title = "Issue 2", AssigneeId = user.Id },
            new() { Id = Guid.NewGuid(), Title = "Issue 3", AssigneeId = user.Id }
        };

        // Act
        user.AssignedIssues.AddRange(issues);

        // Assert
        user.AssignedIssues.Should().HaveCount(3);
    }

    [Theory]
    [InlineData("john@example.com")]
    [InlineData("jane.doe@company.co.uk")]
    [InlineData("test.user+tag@domain.com")]
    public void User_ShouldSupportVariousEmailFormats(string email)
    {
        // Arrange & Act
        var user = new User { Email = email };

        // Assert
        user.Email.Should().Be(email);
    }
}
