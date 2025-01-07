using Domain.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddAuth(configuration);
        serviceCollection.AddDistributedCommunication(options => {});
        
        serviceCollection.AddScoped<IRouletteService, RouletteService>();
        
        return serviceCollection;
    }
    
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseAuthentication();
        applicationBuilder.UseAuthorization();
        
        return applicationBuilder;
    }
}