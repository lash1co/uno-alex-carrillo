using IssueTracker.Domain.Entities;
using Xunit;
using FluentAssertions;
using System;

namespace IssueTracker.Tests.Domain.Entities;

public class AttachmentTests
{
    [Fact]
    public void Attachment_ShouldHaveDefaultValues_WhenCreated()
    {
        // Arrange & Act
        var attachment = new Attachment();

        // Assert
        attachment.Id.Should().Be(Guid.Empty);
        attachment.FileName.Should().Be(string.Empty);
        attachment.FilePath.Should().Be(string.Empty);
        attachment.UploadedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        attachment.IssueId.Should().Be(Guid.Empty);
    }

    [Fact]
    public void Attachment_ShouldAllowSettingProperties()
    {
        // Arrange
        var attachment = new Attachment();
        var attachmentId = Guid.NewGuid();
        var issueId = Guid.NewGuid();
        var fileName = "screenshot.png";
        var filePath = "/uploads/screenshot.png";

        // Act
        attachment.Id = attachmentId;
        attachment.FileName = fileName;
        attachment.FilePath = filePath;
        attachment.IssueId = issueId;

        // Assert
        attachment.Id.Should().Be(attachmentId);
        attachment.FileName.Should().Be(fileName);
        attachment.FilePath.Should().Be(filePath);
        attachment.IssueId.Should().Be(issueId);
    }

    [Fact]
    public void Attachment_ShouldAllowRelationshipWithIssue()
    {
        // Arrange
        var issue = new Issue { Id = Guid.NewGuid(), Title = "Test Issue" };
        var attachment = new Attachment
        {
            Id = Guid.NewGuid(),
            FileName = "screenshot.png",
            FilePath = "/uploads/screenshot.png",
            IssueId = issue.Id,
            Issue = issue
        };

        // Act & Assert
        attachment.Issue.Should().NotBeNull();
        attachment.Issue?.Id.Should().Be(issue.Id);
        attachment.IssueId.Should().Be(issue.Id);
    }

    [Fact]
    public void Attachment_Domain_AllowsAnyFileType_ValidationOccursInApplicationLayer()
    {
        // Note: The Domain entity allows any file type to be set, but validation of allowed extensions
        // (.jpg, .jpeg, .png) is enforced at the Application/Infrastructure layer
        // This test documents that constraint and ensures non-image files are properly handled

        // Arrange & Act - Domain allows setting any file type
        var nonImageAttachment = new Attachment { FileName = "document.pdf" };

        // Assert - Domain level doesn't validate, but value is stored correctly
        nonImageAttachment.FileName.Should().Be("document.pdf");
    }

    [Theory]
    [InlineData("image.jpg")]
    [InlineData("screenshot.jpeg")]
    [InlineData("diagram.png")]
    public void Attachment_ShouldSupportValidImageFileTypes(string fileName)
    {
        // Arrange & Act
        var attachment = new Attachment { FileName = fileName };

        // Assert
        attachment.FileName.Should().Be(fileName);
    }
}
