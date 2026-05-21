using IssueTracker.Api.Utils;
using Scalar.AspNetCore;

namespace IssueTracker.Api.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApiDocumentation(
        this IServiceCollection services)
    {
        services.AddOpenApi("v1", options =>
        {
            options.AddDocumentTransformer((document, context, ct) =>
            {
                document.Info = new()
                {
                    Title = "Issue Tracker API",
                    Version = "v1",
                    Description = """
                        Internal issue tracking system API.

                        Test credentials:
                        Email: admin@test.com
                        Password: admin123
                        """
                };
                return Task.CompletedTask;
            });

            // Seguridad Bearer/JWT
            options.AddDocumentTransformer<BearerSecurityTransformer>();
        });

        return services;
    }

    public static IApplicationBuilder UseOpenApiDocumentation(
        this WebApplication app)
    {
        app.MapOpenApi();                          // expone /openapi/v1.json
        app.MapScalarApiReference(opts =>          // UI en /scalar/v1
        {
            opts.Title = "Issue Tracker API";
            opts.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });

        return app;
    }
}
