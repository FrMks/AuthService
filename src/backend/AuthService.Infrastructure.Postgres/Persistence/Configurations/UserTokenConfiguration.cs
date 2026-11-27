using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Postgres.Persistence.Configurations;

// Токены Identity (например, подтверждение email или сброс пароля) привязаны к
// пользователю и паре provider/name, которая образует составной ключ таблицы.
public sealed class UserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("user_tokens", "auth");

        builder.Property(token => token.UserId).HasColumnName("user_id");
        builder.Property(token => token.LoginProvider).HasColumnName("login_provider");
        builder.Property(token => token.Name).HasColumnName("name");
        builder.Property(token => token.Value).HasColumnName("value");
    }
}
