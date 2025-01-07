using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddMediatR(this IServiceCollection serviceCollection, Assembly assembly)
    {
        serviceCollection.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
        });
        
        return serviceCollection;
    }
}