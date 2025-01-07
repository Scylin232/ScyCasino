using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        Assembly assembly = typeof(DependencyInjection).Assembly;
        
        serviceCollection.AddMediatR(assembly);
        
        return serviceCollection;
    }
}