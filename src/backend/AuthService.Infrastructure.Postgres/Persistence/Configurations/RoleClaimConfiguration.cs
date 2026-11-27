using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Postgres.Persistence.Configurations;

// Claims роли описывают разрешения или атрибуты роли и хранятся отдельно от самой роли,
// чтобы назначать каждой роли набор утверждений без расширения таблицы roles.
public sealed class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("role_claims", "auth");

        builder.Property(claim => claim.Id).HasColumnName("id");
        builder.Property(claim => claim.RoleId).HasColumnName("role_id");
        builder.Property(claim => claim.ClaimType).HasColumnName("claim_type");
        builder.Property(claim => claim.ClaimValue).HasColumnName("claim_value");
    }
}
