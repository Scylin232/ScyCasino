using Application.Consumers.Room;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Repositories;
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
        serviceCollection.AddRedisStorage();
        serviceCollection.AddDistributedCommunication(options =>
        {
            options.AddConsumer<ValidateUserRoomConsumer>();
        });
        
        serviceCollection.AddSignalR(options => { 
            options.EnableDetailedErrors = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"; 
        });
        
        serviceCollection.AddScoped<IRoomRepository, RoomRepository>();
        
        serviceCollection.AddHttpClient();
        serviceCollection.AddScoped<IUserService, UserService>();
        
        return serviceCollection;
    }
    
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseAuthentication();
        applicationBuilder.UseAuthorization();

        return applicationBuilder;
    }
}