using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Postgres.Persistence;

public sealed class AuthDbContext(
    DbContextOptions<AuthDbContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Дефолтная схема - auth, чтобы все таблицы создавались в ней, а не в public
        modelBuilder.HasDefaultSchema("auth");

        base.OnModelCreating(modelBuilder);
    }
}
