

using Microsoft.Extensions.DependencyInjection;
using Btech.Package.EntityFramework.Contracts;
using Btech.Package.EntityFramework.Interfaces; 

namespace IQT_FSD_2026.Infrastructure;

public static class DependencyInjections
{
    public static IServiceCollection AddInfrastructureDependencyInjections(this IServiceCollection services)
    {   
        return services;
    }
}
