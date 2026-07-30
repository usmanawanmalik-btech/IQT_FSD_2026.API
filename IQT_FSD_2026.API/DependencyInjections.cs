
using System.Reflection;
using System.Text.Json;
 
using Btech.Package.Domain;
using Btech.Package.Domain.Common;
using Btech.Package.Domain.Filter;
using Btech.Package.Domain.Services.Authentication;
using Btech.Package.EntityFramework; 

using IQT_FSD_2026.Application;
using IQT_FSD_2026.Domain;
using IQT_FSD_2026.EFMigration.MySQL;
using IQT_FSD_2026.EFMigration.PostgreSQL;
using IQT_FSD_2026.EFMigration.SQLServer;
using IQT_FSD_2026.Infrastructure;
using IQT_FSD_2026.Infrastructure.DbContexts;

namespace IQT_FSD_2026.WebAPI;

public static class DependencyInjections
{
    public static IServiceCollection AddWebAPIDependencyInjections(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var appSettings = serviceProvider.GetRequiredService<AppSettings>();

        services
            .AddEntityFrameworkDbFactoryServices<ApplicationDbContext, PostgreSQLMigrationAssembly, SQLServerMigrationAssembly, MySQLMigrationAssembly, DbSeedingFactory>(appSettings.EnableMigrations);

        services.AddDomainDependencyInjections(); 

        //services.AddInventoryInfrastructureDependencyInjections(); 
        //services.AddInventoryApplicationDependencyInjections();
         
         
        // Add services to the container.  
        services
            .AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.Filters.Add(typeof(ValidateFilterAttribute)); 
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            })
            .AddNewtonsoftJson(
                   options =>
                   {
                       options.SerializerSettings.NullValueHandling =
                        Newtonsoft.Json.NullValueHandling.Ignore;
                       options.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                   }
               );


        List<Assembly> assemblies = Assembly.GetExecutingAssembly()
            .GetReferenceAssembliesFromPath(new string[] { "btech.*.dll", "IQT_FSD_2026.*.dll" });
        services.AddApplicationServicesAndRepositoriesDependencies(assemblies);

        //services.AddAuthenticationServiceV1(appSettings.AuthenticationOptions);
        services.AddSwaggerWithAuthCheckAndSecurityV1(appSettings.ApplicationDetails.ApplicationName, appSettings.SwaggerAuthenticationOptions);
          
        services.AddApplicationDependencyInjections();

        return services;
    }
}
