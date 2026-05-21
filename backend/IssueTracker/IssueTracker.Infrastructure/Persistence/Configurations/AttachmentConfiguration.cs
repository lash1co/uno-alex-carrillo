using IssueTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IssueTracker.Infrastructure.Persistence.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(a => a.UploadedAt)
            .IsRequired()
            .HasDefaultValueSql("now()");

        builder.Property(a => a.IssueId)
            .IsRequired();

        builder.HasOne(a => a.Issue)
            .WithMany(i => i.Attachments)
            .HasForeignKey(a => a.IssueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}