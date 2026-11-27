using AuthService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Postgres.Persistence.Configurations;

// Role расширяет IdentityRole<Guid>; роли хранятся отдельно от пользователей,
// а членство пользователей в ролях связывается таблицей user_roles.
public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("roles", "auth");

        builder.Property(role => role.Id).HasColumnName("id");
        builder.Property(role => role.Name).HasColumnName("name");
        builder.Property(role => role.NormalizedName).HasColumnName("normalized_name");
        builder.Property(role => role.ConcurrencyStamp).HasColumnName("concurrency_stamp");
    }
}
