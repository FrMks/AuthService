# AuthService

Заготовка микросервисного стека AuthService: .NET 10 backend, PostgreSQL, Next.js frontend и Nginx.

В проекте пока нет бизнес-логики аутентификации. Есть демонстрационная feature, инфраструктурная заготовка и готовый dev-стек.

## Быстрый запуск

Требования: Docker Desktop, .NET SDK 10 и Node.js для локальной разработки.

Из корня репозитория выполните:

```powershell
Copy-Item .env.example .env
docker build --secret id=github_pat,env=GITHUB_PAT --secret id=github_user,env=GITHUB_USER -f Dockerfile.backend -t authservice-backend .
docker compose up --build
```

Для сборки backend-образа нужны переменные `GITHUB_USER` и `GITHUB_PAT`: Shared-пакеты загружаются из GitHub Packages. PAT должен иметь право `read:packages` и не должен добавляться в Git.

После запуска:

- frontend: <http://localhost>;
- health check через Nginx: <http://localhost/api/health>;
- demo API через Nginx: <http://localhost/api/demo>;
- Swagger backend: <http://localhost:8080/swagger/index.html>.

Остановка стека:

```powershell
docker compose down
```

Остановка с удалением локального тома PostgreSQL:

```powershell
docker compose down -v
```

## Как устроен запуск

`postgres` запускает PostgreSQL и становится healthy после `pg_isready`.

`migrations` запускает `/app/efbundle` после готовности PostgreSQL. Это отдельный one-shot контейнер: он применяет EF Core migrations и завершается с кодом 0.

`authservice` стартует только после успешного завершения контейнера миграций.

`frontend` запускает Next.js в режиме разработки на порту 3000 с поддержкой HMR.

`nginx` является единой точкой входа на порту 80: `/api/*` направляется в backend с удалением префикса `/api`, остальные запросы — во frontend.

## Локальная разработка

Backend:

```powershell
dotnet restore
dotnet build
dotnet test
```

Frontend:

```powershell
Set-Location src/frontend
npm ci
npm run dev
```

Проверки frontend:

```powershell
npm run lint
npm run build
```

## Структура проекта

```text
src/
├── backend/
│   ├── AuthService.Api/                     # HTTP API, controllers, middleware, Swagger
│   ├── AuthService.Application/             # Vertical Slice features и Wolverine handlers
│   ├── AuthService.Contracts/               # DTO и контракты API
│   ├── AuthService.Domain/                  # Domain layer
│   └── AuthService.Infrastructure.Postgres/ # EF Core DbContext и migrations
└── frontend/                                # Next.js App Router + TypeScript + FSD
    ├── app/
    ├── widgets/
    ├── features/
    ├── entities/
    └── shared/

tests/
├── AuthService.UnitTests/
└── AuthService.IntegrationTests/

infra/nginx/nginx.conf       # Dev reverse proxy
Dockerfile.backend           # Backend и efbundle image
Dockerfile.frontend          # Frontend dev image
docker-compose.yml           # PostgreSQL, migrations, API, frontend, Nginx
Directory.Build.props        # Общие настройки компилятора и analyzers
Directory.Packages.props     # Центральное управление версиями NuGet
global.json                  # Фиксация .NET SDK
nuget.config                 # NuGet.org и GitHub Packages
```

## Конфигурация и секреты

Конфигурация backend загружается по слоям:

1. `appsettings.json` — общие безопасные значения;
2. `appsettings.{Environment}.json` — настройки окружения;
3. переменные окружения — финальные значения и секреты.

Файл `.env.example` хранится в Git как шаблон. Файл `.env` игнорируется и должен содержать только локальные значения.

## Архитектурные правила

- Бизнес-ошибки возвращаются через Result/Error pattern, а не через исключения.
- Необработанные исключения обрабатываются глобальным middleware.
- Serilog пишет request ID, trace ID, маршрут, статус и длительность запроса.
- Health check проверяет доступность PostgreSQL через EF Core.
- Миграции не выполняются при старте API — только отдельным контейнером `migrations`.
- Wolverine используется как dispatcher для command/query handlers.
