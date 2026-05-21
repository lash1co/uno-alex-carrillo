using FluentValidation;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Services;
using IssueTracker.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IssueTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
            cfg.AddMaps(typeof(DependencyInjection).Assembly));

        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly,
            includeInternalTypes: true);

        services.AddScoped<
            IIssueService,
            IssueService>();

        services.AddScoped<
            IAssigneeService,
            AssigneeService>();

        // Register authentication service
        services.AddScoped<
            IAuthenticationService,
            AuthenticationService>();

        return services;
    }
}
