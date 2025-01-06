using Domain.Repositories;
using Domain.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Repositories;
using StackExchange.Redis;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddAuth(configuration);
        serviceCollection.AddDataStorage();
        
        return serviceCollection;
    }
    
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseAuthentication();
        applicationBuilder.UseAuthorization();

        return applicationBuilder;
    }
    
    private static IServiceCollection AddAuth(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        string? authority = configuration["JWT:Authority"];
        string? audience = configuration["JWT:Audience"];
        string? issuer = configuration["JWT:Issuer"];
        
        bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        
        serviceCollection
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.Audience = audience;
                
                options.IncludeErrorDetails = isDevelopment;
                options.RequireHttpsMetadata = !isDevelopment;
                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        string? accessToken = context.Request.Query["access_token"];
                        
                        PathString path = context.HttpContext.Request.Path;
                        
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/room-hub"))
                            context.Token = accessToken;
                        
                        return Task.CompletedTask;
                    }
                };
            });
        
        serviceCollection
            .AddAuthorization(options =>
            {
                options.AddPolicy("IsAdministrator", policy =>
                    policy.RequireRole("ScyCasino Administrator"));
            });
        
        return serviceCollection;
    }

    private static IServiceCollection AddDataStorage(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSignalR(options => { 
            options.EnableDetailedErrors = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"; 
        });
        
        serviceCollection.AddSingleton<IConnectionMultiplexer>(_ => 
            ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")));
        serviceCollection.AddScoped<RedisUnitOfWork>();
        
        serviceCollection.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<RedisUnitOfWork>());
        
        serviceCollection.AddScoped<IRoomRepository, RoomRepository>();
        
        serviceCollection.AddHttpClient();
        serviceCollection.AddScoped<IUserService, UserService>();
        
        return serviceCollection;
    }
}