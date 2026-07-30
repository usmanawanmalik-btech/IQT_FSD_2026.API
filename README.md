# IQT-FSD-2026

An ASP.NET Core Web API for the IQT_FSD_2026 domain. This solution implements REST endpoints, Entity Framework database migrations for multiple RDBMS providers, Redis caching, structured logging, and OpenAPI (Swagger) with OAuth integration. It is organized as a multi-project solution (API, Application, Domain, Infrastructure).

## Quick overview

- Technology: .NET (ASP.NET Core), C#
- Projects: IQT_FSD_2026.API (Web API), IQT_FSD_2026.Application (application services, caching), IQT_FSD_2026.Domain (DTOs/domain services), IQT_FSD_2026.Infrastructure (DbContexts/data access), EF migration assemblies for PostgreSQL / SQL Server / MySQL
- Notable packages: Serilog, Entity Framework, Swashbuckle (Swagger), Newtonsoft.Json, Redis caching (via Btech.Package.Cache), and internal/shared Btech.Package.* libraries

## Repository structure

Important top-level entries:

```
IQT_FSD_2026.API.sln              # Solution file
IQT_FSD_2026.API/                 # Web API project (Program.cs, controllers, DI)
IQT_FSD_2026.Application/         # Application services, caching DI
IQT_FSD_2026.Domain/              # Domain DTOs and domain DI
IQT_FSD_2026.Infrastructure/      # DbContexts and infrastructure code
IQT_FSD_2026.EFMigration.PostgreSQL/   # EF migration assembly (Postgres)
IQT_FSD_2026.EFMigration.SQLServer/    # EF migration assembly (SQL Server)
IQT_FSD_2026.EFMigration.MySQL/        # EF migration assembly (MySQL)
Databases/                        # (optional) database scripts
README.md
NuGet.Config
.github/
```

How the pieces fit:
- The API project (Program.cs) wires Serilog, tracing, CORS and calls extension methods to register the domain and application services.
- DependencyInjections.cs in the API registers an EF DbFactory with migration assemblies for PostgreSQL/SQL Server/MySQL, configures MVC controllers, JSON options and Swagger with OAuth settings from AppSettings.
- Application project configures caching (Redis by default) and exposes application-layer services used by controllers.
- Domain contains DTOs, filters and domain-level DI. Infrastructure contains DbContexts and persistence implementations consumed through the EF factory.

## Configuration

Settings are read from `IQT_FSD_2026.API/appsettings.json` and `appsettings.{ENV}.json`. Key sections to review/override before running:

- AppSettings: application metadata and SwaggerOAuth options
- ConnectionStrings / database settings: used by the EF DbFactory
- LogConfig: Serilog settings
- Redis settings: used by the caching configuration

Program.cs binds AppSettings and provides them to DI. `DependencyInjections.cs` relies on those values to register database, migrations and Swagger/OAuth.

## Run locally

Prerequisites:
- .NET SDK (6.0+)
- A database of your choice (Postgres, SQL Server, or MySQL) and correct connection string in configuration
- Redis if you want caching (or change provider in code)

Restore, build, run (from repository root):

```bash
dotnet restore IQT_FSD_2026.API.sln
dotnet build IQT_FSD_2026.API.sln -c Debug
dotnet run --project IQT_FSD_2026.API
```

Set environment and connection strings as needed. Example (PowerShell / Windows cmd-style shown in repo):

```bash
set ASPNETCORE_ENVIRONMENT=Development
set ConnectionStrings__Default="Host=localhost;Database=iqt;Username=...;Password=..."
set Redis__Connection="localhost:6379"
dotnet run --project IQT_FSD_2026.API
```

The API exposes Swagger UI configured with OAuth (options come from AppSettings). By default controllers are mounted and endpoints are available under the application's base URL.

## Migrations

This solution uses an EF migration factory that is configured with migration assemblies for PostgreSQL, SQL Server and MySQL. Migration assemblies included/referenced in the solution:

- IQT_FSD_2026.EFMigration.PostgreSQL
- IQT_FSD_2026.EFMigration.SQLServer
- IQT_FSD_2026.EFMigration.MySQL

Apply migrations using the approach appropriate for your chosen provider. The DI wiring expects a factory to select the correct provider at runtime.

## Development notes

- Logging: Serilog is set up in Program.cs. Check `LogConfig` in appsettings for sinks/levels.
- JSON: Controllers are configured to use camelCase naming and both System.Text.Json and Newtonsoft.Json settings are present.
- CORS: A permissive CORS policy `BtechCors` is added in Program.cs.
- Shared/internal packages: the solution references `Btech.Package.*` libraries (Domain, EntityFramework, Cache, Logger). Ensure these are available via configured NuGet feed or local packages (see `NuGet.Config`).

## Useful commands

- Publish (Windows): `IQT_FSD_2026.API/publish_command.bat`
- Run solution tests (if any): (not present in repo) — add test project(s) and run `dotnet test`.

## Next steps / questions

If you'd like I can:
- Open `IQT_FSD_2026.API/appsettings.json` and highlight required environment-specific values (DB connection, Swagger OAuth client/secret, Redis connection).
- Show the EF migration projects and a recommended `dotnet ef` command for applying migrations to Postgres/SQL Server/MySQL.
- Add a short CONTRIBUTING section and sample Docker run instructions.

