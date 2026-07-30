 
using Btech.Package.Domain;
using Btech.Package.Domain.Services.Authentication;
using Btech.Package.EntityFramework;

using Btech.Package.Logger;
using Serilog;

namespace IQT_FSD_2026.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var corsPolicy = "BtechCors";

            var builder = WebApplication.CreateBuilder(args);

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")??"";
            var isDevelopment = environment == Environments.Development;
             
            var configuration = GetConfiguration(args, environment);
            //builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

            var appSettings = new AppSettings();
            builder.Configuration.GetSection("AppSettings").Bind(appSettings);
            builder.Services.AddScoped((cfg) => { return appSettings; });

            var logConfig = new LogConfig();
            builder.Configuration.GetSection("LogConfig").Bind(logConfig);

            //builder.Services.AddCustomTelemetry(builder, builder.Configuration); 
            builder.Host.UseSerilog((context, loggerConfiguration) => SeriLogger.Configure(context, loggerConfiguration));
            builder.Services.AddTracing(builder, builder.Configuration);

            //Add Domain Default Services
            builder.Services.AddDomainDefaultDependencyInjections();
             
            //Add Web API Services
            builder.Services.AddWebAPIDependencyInjections();

            //Configuring the Authorization Service
            builder.Services.AddAuthorization();

            //Add Cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corsPolicy, policy =>
                {
                    policy
                        .WithOrigins("*")
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseExceptionHandler("/Error");
                //app.UseHsts(); 
            }

            //Todo: Add logging middleware here 
            app.UseUserContextMiddleware();
            //app.UseSerilogHttpSessionsLogging(HttpSessionInfoToLog.All);

            app.UseDeveloperExceptionPage();
            //app.UseStaticFiles();
            app.UseCors(corsPolicy);
            app.UseRouting();

            //app.UseAuthentication();
           // app.UseAuthorization();
             
            app.UseSwagger()
                .UseSwaggerUI(options =>
                {
                    //options.RoutePrefix = "swagger";
                    //options.SwaggerEndpoint("/api/swagger/v1/swagger.json", appSettings.ApplicationDetails.ApplicationName);

                    options.DefaultModelsExpandDepth(-1);
                    options.OAuthClientId(appSettings.SwaggerAuthenticationOptions.ClientId);
                    options.OAuthScopes(appSettings.SwaggerAuthenticationOptions.Scopes.Split(' '));
                    if (!string.IsNullOrEmpty(appSettings.SwaggerAuthenticationOptions.Secret)) 
                        options.OAuthClientSecret(appSettings.SwaggerAuthenticationOptions.Secret);
                    options.OAuthUsePkce();
                });

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.Run();
        }

        private static IConfiguration GetConfiguration(string[] args, string environment)
        { 
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true); 

            var configuration = configurationBuilder.Build(); 

            configurationBuilder.AddCommandLine(args);
            configurationBuilder.AddEnvironmentVariables();

            return configurationBuilder.Build();
        } 
    }
}
