using Microsoft.Extensions.FileProviders;

namespace IssueTracker.Api.Extensions;

public static class StaticFilesExtensions
{
    public static IApplicationBuilder
        UseUploadsStaticFiles(
            this IApplicationBuilder app)
    {
        var uploadsPath =
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "uploads");

        Directory.CreateDirectory(
            uploadsPath);

        app.UseStaticFiles(
            new StaticFileOptions
            {
                FileProvider =
                    new PhysicalFileProvider(
                        uploadsPath),

                RequestPath = "/uploads"
            });

        return app;
    }
}
