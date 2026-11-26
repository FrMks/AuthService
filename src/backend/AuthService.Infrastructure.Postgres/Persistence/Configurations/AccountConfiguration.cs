using AuthService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Postgres.Persistence.Configurations;

// Account расширяет IdentityUser<Guid>; конфигурация переводит встроенные поля
// учётной записи в таблицу users со snake_case именами колонок.
public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ToTable("users", "auth");

        builder.Property(account => account.Id).HasColumnName("id");
        builder.Property(account => account.UserName).HasColumnName("user_name");
        builder.Property(account => account.NormalizedUserName).HasColumnName("normalized_user_name");
        builder.Property(account => account.Email).HasColumnName("email");
        builder.Property(account => account.NormalizedEmail).HasColumnName("normalized_email");
        builder.Property(account => account.EmailConfirmed).HasColumnName("email_confirmed");
        builder.Property(account => account.PasswordHash).HasColumnName("password_hash");
        builder.Property(account => account.SecurityStamp).HasColumnName("security_stamp");
        builder.Property(account => account.ConcurrencyStamp).HasColumnName("concurrency_stamp");
        builder.Property(account => account.PhoneNumber).HasColumnName("phone_number");
        builder.Property(account => account.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
        builder.Property(account => account.TwoFactorEnabled).HasColumnName("two_factor_enabled");
        builder.Property(account => account.LockoutEnd).HasColumnName("lockout_end");
        builder.Property(account => account.LockoutEnabled).HasColumnName("lockout_enabled");
        builder.Property(account => account.AccessFailedCount).HasColumnName("access_failed_count");
    }
}
