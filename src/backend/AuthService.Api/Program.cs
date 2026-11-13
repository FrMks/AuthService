using AuthService.Infrastructure.Postgres.Persistence;
using AuthService.Application.Features.Demo.GetDemo;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Framework.Middlewares;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

// Подключаем Serilog как основной провайдер логирования приложения
// Настройки Serilog читаются из appsettings.json и переменных окружения
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        // Читаем секциб "Serilog" из конфигурации приложения
        .ReadFrom.Configuration(context.Configuration)
        // Добавляем в логи значения из LogContext:
        // TraceId, RequestId и другие свойства запроса
        .Enrich.FromLogContext();
});

builder.Services.AddDbContext<AuthDbContext>(options =>
{
   options.UseNpgsql(
        builder.Configuration.GetConnectionString("AuthDbConnection"));
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Подключаем Wolverine как mediator для command/query сообщений
builder.Host.UseWolverine(options =>
{
    // Handler находится в Application assembly,
    // а не в основном API assembly
    options.ApplicationAssembly = typeof(GetDemoQueryHandler).Assembly;

    // Пока используем Wolverine только внутри процесса
    // RabbitMQ, очереди и durable messaging пока не включаем
    options.Durability.Mode = DurabilityMode.MediatorOnly;

    // Включаем подробные Wolverine traces для InvokeAsync
    options.InvokeTracing = InvokeTracingMode.Full;
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Глобально обрабатываем непредвиденные исключения.
// Middleware формирует единый формат ошибки и добавляет trace ID
app.UseSharedExceptionHandling();

// Это middleware, который будет перехватывать все необработанные исключения в приложении и логировать их через Serilog.
// Логируем каждый HTTP-запрос через Serilog
// Http-методы, путь, статус ответа и длительность выполнения
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate =
        "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

    // EnrichDiagnosticContext позволяет добавлять в лог дополнительные свойства, которые будут доступны в шаблоне сообщения.
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        // TraceIdentifier - это уникальный идентификатор запроса, который генерируется ASP.NET Core для каждого HTTP-запроса. Он полезен для трассировки запросов в логах.
        diagnosticContext.Set(
            "RequestId",
            httpContext.TraceIdentifier);

        // TraceId - это идентификатор трассировки, который может быть использован для корреляции логов между различными сервисами в распределённой системе. В ASP.NET Core он совпадает с TraceIdentifier, но в других системах может быть другим.
        diagnosticContext.Set(
            "TraceId",
            httpContext.TraceIdentifier);
    };
});

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
