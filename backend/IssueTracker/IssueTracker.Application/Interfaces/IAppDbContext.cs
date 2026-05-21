using IssueTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Issue> Issues { get; }

    DbSet<Attachment> Attachments { get; }

    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
