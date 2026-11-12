using AuthService.Infrastructure.Postgres.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthDbContext>(options =>
{
   options.UseNpgsql(
        builder.Configuration.GetConnectionString("AuthDbConnection"));
});

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    service = "AuthService",
    status = "running"
}));

app.Run();
