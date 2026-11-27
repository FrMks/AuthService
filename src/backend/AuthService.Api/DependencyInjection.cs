using AuthService.Domain;
using AuthService.Infrastructure.Postgres;
using AuthService.Infrastructure.Postgres.Persistence;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Api;

/// <summary>
/// Composes service registrations for the AuthService API host.
/// </summary>
internal static class ApiServiceCollectionExtensions
{
    public static IServiceCollection AddProgramDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddPostgresInfrastructure(configuration);

        // Регистрирует UserManager, SignInManager и другие сервисы ASP.NET Core Identity.
        services
        // Регистрирует сервисы Identity:
        // UserManager управляет пользователями: создаёт аккаунты, проверяет пароли,
        // меняет email и сбрасывает пароли.
        // SignInManager выполняет вход, проверяет lockout и управляет входом по паролю
        // или временным двухфакторным кодам.
            .AddIdentity<Account, Role>(options =>
            {
                // Пароль должен содержать не менее 8 символов.
                options.Password.RequiredLength = 8;

                // Блокирует вход на 15 минут после 5 неудачных попыток.
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

                // Запрещает создавать несколько аккаунтов с одним email.
                options.User.RequireUniqueEmail = true;
            })
            // Подключает хранение пользователей и ролей через EF Core и AuthDbContext.
            .AddEntityFrameworkStores<AuthDbContext>()
            // Подключает стандартную генерацию и проверку одноразовых кодов Identity,
            // например для ссылок подтверждения email и сброса пароля.
            .AddDefaultTokenProviders();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}
