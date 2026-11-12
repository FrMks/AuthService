using AuthService.Infrastructure.Postgres.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthDbContext>(options =>
{
   options.UseNpgsql(
        builder.Configuration.GetConnectionString("AuthDbConnection"));
});

// Health checks - это встроенная проверка: "сервис сейчас работает нормально?"
// Например, приложение может быть запущено, но PostgreSQL недоступен. 
// В таком случае процесс жив, но сервис фактически не готов работать
builder.Services
    .AddHealthChecks() // Подключает в приложение систему проверок здоровья.
    .AddDbContextCheck<AuthDbContext>( // Добавляем конкретную проверку PostgreSQL через DbContext. Проверка будет пытаться подключиться к БД и выполнить простейший запрос.
        name: "postgres", // Имя проверки, которое будет отображаться в результатах
        tags: ["ready"]); // Теги нужны для того, чтобы можно было запускать только определённые проверки. Например, мы можем запускать только проверки с тегом "ready", когда сервис запускается в Kubernetes, а проверки с тегом "live" - когда Kubernetes проверяет, что процесс жив и не завис.

var app = builder.Build();

app.MapHealthChecks("/health");

app.MapGet("/", () => Results.Ok(new
{
    service = "AuthService",
    status = "running"
}));

app.Run();
