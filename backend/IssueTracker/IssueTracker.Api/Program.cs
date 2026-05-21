using IssueTracker.Application;
using IssueTracker.Infrastructure;
using IssueTracker.Api.Extensions;
using IssueTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Application layer
builder.Services.AddApplication();

// Infrastructure layer
builder.Services.AddInfrastructure(builder.Configuration);

// API layer - Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// API layer - CORS
builder.Services.AddCorsPolicy(builder.Configuration);

// API layer - Swagger/OpenAPI
builder.Services.AddOpenApiDocumentation();

// Controllers
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Global exception handling middleware
app.UseGlobalExceptionMiddleware();

// Swagger documentation
if (app.Environment.IsDevelopment())
{
    app.UseOpenApiDocumentation();
}

// HTTPS redirect
app.UseHttpsRedirection();

app.UseUploadsStaticFiles();

// CORS
app.UseCors(
    CorsExtensions.PolicyName);

// Authentication and Authorization
app.UseAuthentication();

app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();
