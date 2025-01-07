using Application.Consumers.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Shared.Infrastructure;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddAuth(configuration);
        serviceCollection.AddEntityFrameworkStorage<DataContext>(options =>
        {
            options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING_DEVELOPMENT"));
        });
        serviceCollection.AddDistributedCommunication(options =>
        {
            options.AddConsumer<ValidateAccessTokenConsumer>();
        });
        
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        
        return serviceCollection;
    }
    
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseAuthentication();
        applicationBuilder.UseAuthorization();
        
        return applicationBuilder;
    }
}