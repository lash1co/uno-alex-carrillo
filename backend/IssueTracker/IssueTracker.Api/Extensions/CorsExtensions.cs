namespace IssueTracker.Api.Extensions;

public static class CorsExtensions
{
    public const string PolicyName = "CorsPolicy";

    public static IServiceCollection AddCorsPolicy(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var frontendUrl =
            configuration["Frontend:Url"]
            ?? "http://localhost:8080";

        services.AddCors(options =>
        {
            options.AddPolicy(
                PolicyName,
                policy =>
                {
                    policy
                        .WithOrigins(frontendUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        return services;
    }
}
