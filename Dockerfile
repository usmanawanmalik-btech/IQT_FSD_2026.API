# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.Config", "."] 
COPY ["IQT_FSD_2026.Infrastructure/_Pharmacy_Product_Seeding/pharmacy_medicien_list.xlsx", "./_Pharmacy_Product_Seeding/"]
COPY ["IQT_FSD_2026.WebAPI/IQT_FSD_2026.WebAPI.csproj", "IQT_FSD_2026.WebAPI/"]
COPY ["Databases/IQT_FSD_2026.EFMigration.MySQL/IQT_FSD_2026.EFMigration.MySQL.csproj", "Databases/IQT_FSD_2026.EFMigration.MySQL/"]
COPY ["IQT_FSD_2026.Infrastructure/IQT_FSD_2026.Infrastructure.csproj", "IQT_FSD_2026.Infrastructure/"]
COPY ["Databases/IQT_FSD_2026.EFMigration.PostgreSQL/IQT_FSD_2026.EFMigration.PostgreSQL.csproj", "Databases/IQT_FSD_2026.EFMigration.PostgreSQL/"]
COPY ["Databases/IQT_FSD_2026.EFMigration.SQLServer/IQT_FSD_2026.EFMigration.SQLServer.csproj", "Databases/IQT_FSD_2026.EFMigration.SQLServer/"]
COPY ["IQT_FSD_2026.Application/IQT_FSD_2026.Application.csproj", "IQT_FSD_2026.Application/"]
COPY ["IQT_FSD_2026.Domain/IQT_FSD_2026.Domain.csproj", "IQT_FSD_2026.Domain/"]
COPY ["IQT_FSD_2026.Reports/IQT_FSD_2026.Reports.csproj", "IQT_FSD_2026.Reports/"]
RUN dotnet restore "./IQT_FSD_2026.WebAPI/IQT_FSD_2026.WebAPI.csproj"
COPY . .
WORKDIR "/src/IQT_FSD_2026.WebAPI"
RUN dotnet build "./IQT_FSD_2026.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IQT_FSD_2026.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IQT_FSD_2026.WebAPI.dll"]