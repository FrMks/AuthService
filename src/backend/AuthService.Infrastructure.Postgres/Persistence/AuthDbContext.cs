using AuthService.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Postgres.Persistence;

public sealed class AuthDbContext(
    DbContextOptions<AuthDbContext> options)
    : IdentityDbContext<Account, Role, Guid>(options)
{
    // IdentityDbContext уже регистрирует все семь Identity-сущностей в EF-модели.
    // Контекст уже предоставляет Users, Roles и наборы для встроенных Identity-сущностей.
    // Accounts — удобный прикладной алиас к Users для кода AuthService.
    public DbSet<Account> Accounts => Set<Account>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(
            builder ?? throw new ArgumentNullException(nameof(builder)));

        // Identity состоит из пользовательской и ролевой сущностей и связанных с ними
        // таблиц claims, связей ролей, внешних логинов и токенов.
        // Общий default schema задаёт схему для всей модели; конфигурации ниже
        // закрепляют snake_case имена таблиц и колонок для каждой Identity-сущности.
        builder.HasDefaultSchema("auth");

        builder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
    }
}
