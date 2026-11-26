using AuthService.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Postgres.Persistence;

public sealed class AuthDbContext(
    DbContextOptions<AuthDbContext> options)
    : IdentityDbContext<Account, Role, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Дефолтная схема - auth, чтобы все таблицы создавались в ней, а не в public
        builder.HasDefaultSchema("auth");
    }
}
