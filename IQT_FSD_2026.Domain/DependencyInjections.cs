
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;  

namespace IQT_FSD_2026.Domain;

public static class DependencyInjections
{
    public static IServiceCollection AddDomainDependencyInjections(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjections).Assembly;
        services.AddValidatorsFromAssembly(assembly);

        //services.AddValidatorsFromAssemblyContaining<BrandValidator>();
        //services.AddValidatorsFromAssemblyContaining<CategoryValidator>();
        return services;
    }
}
