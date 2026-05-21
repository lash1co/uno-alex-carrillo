using IssueTracker.Application.Common.Options;
using IssueTracker.Application.Interfaces;
using IssueTracker.Infrastructure.Persistence;
using IssueTracker.Infrastructure.Repositories;
using IssueTracker.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IssueTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<FileUploadOptions>(
            configuration.GetSection("FileUpload"));

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString(
                    "DefaultConnection"));
        });

        // Register DbContext as IAppDbContext for Application layer
        services.AddScoped(sp =>
            (IAppDbContext)sp.GetRequiredService<AppDbContext>());

        // Register generic repository implementing Application abstract interface
        services.AddScoped(
            typeof(IRepository<>),
            typeof(Repository<>));

        // Register file storage service
        services.AddScoped<LocalFileStorageService>();
        services.AddScoped<IFileStorageService>(sp =>
            sp.GetRequiredService<LocalFileStorageService>());

        return services;
    }
}
