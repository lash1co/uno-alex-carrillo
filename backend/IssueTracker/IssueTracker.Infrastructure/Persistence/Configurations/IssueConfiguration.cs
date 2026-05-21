using IssueTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssueTracker.Infrastructure.Persistence.Configurations;

public class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(i => i.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(i => i.Priority)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(i => i.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("now()");

        builder.Property(i => i.UpdatedAt)
            .IsRequired(false);

        builder.Property(i => i.AssigneeId)
            .IsRequired(false);

        builder.HasOne(i => i.Assignee)
            .WithMany(u => u.AssignedIssues)
            .HasForeignKey(i => i.AssigneeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(i => i.Attachments)
            .WithOne(a => a.Issue)
            .HasForeignKey(a => a.IssueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}