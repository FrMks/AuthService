using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Postgres.Persistence.Configurations;

// Внешние способы входа (например, Google) хранятся отдельно от локального пароля;
// provider и provider key вместе идентифицируют внешний логин.
public sealed class UserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("user_logins", "auth");

        builder.Property(login => login.LoginProvider).HasColumnName("login_provider");
        builder.Property(login => login.ProviderKey).HasColumnName("provider_key");
        builder.Property(login => login.ProviderDisplayName).HasColumnName("provider_display_name");
        builder.Property(login => login.UserId).HasColumnName("user_id");
    }
}
