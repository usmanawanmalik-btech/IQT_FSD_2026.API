using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Btech.Package.Domain.Services.Context;
using IQT_FSD_2026.EFMigration.MySQL;
using IQT_FSD_2026.EFMigration.PostgreSQL;
using IQT_FSD_2026.EFMigration.SQLServer;
using IQT_FSD_2026.Infrastructure.DbContexts;

namespace IQT_FSD_2026.WebAPI.MigrationFactory
{
    public class DbMigrationFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";
            var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                 .Build();

            var dbParameters = config.GetSection("AppSettings:DbConnectionInfo").Get<DbContextParameter>();
            var connectionString = dbParameters!.BuildConnectionString;

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            if (dbParameters.HostDbModel == DatabaseModel.SQLSERVER)
                optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsAssembly(SQLServerMigrationAssembly.GetMigrationAssemblyByProvider));
            else if (dbParameters.HostDbModel == DatabaseModel.POSTGRESQL)
                optionsBuilder.UseNpgsql(connectionString, x => x.MigrationsAssembly(PostgreSQLMigrationAssembly.GetMigrationAssemblyByProvider));
            else if (dbParameters.HostDbModel == DatabaseModel.MYSQL)
                optionsBuilder.UseMySQL(connectionString, x => x.MigrationsAssembly(MySQLMigrationAssembly.GetMigrationAssemblyByProvider));
            else
            {
                throw new NotSupportedException($"The database model {dbParameters.HostDbModel} is not supported.");
            }
            var initializeDbContext = new ApplicationDbContext(optionsBuilder.Options); 
            return initializeDbContext;
        }
    }
}
