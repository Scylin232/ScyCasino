using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Infrastructure.Data;
using Shared.Kernel.Repositories;
using StackExchange.Redis;

namespace Shared.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDistributedCommunication(this IServiceCollection serviceCollection,  Action<IBusRegistrationConfigurator>? transitOptions)
    {
        string rabbitHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST")!;
        string rabbitUsername = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME")!;
        string rabbitPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")!;
        
        serviceCollection.AddMassTransit(options =>
        {
            transitOptions?.Invoke(options);
            
            options.UsingRabbitMq((context, config) =>
            {
                config.Host(rabbitHost, host =>
                {
                    host.Username(rabbitUsername);
                    host.Password(rabbitPassword);
                });
                
                config.ConfigureEndpoints(context);
            });
        });
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddAuth(this IServiceCollection serviceCollection, IConfiguration configuration)
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
                        
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hub/"))
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
    
    public static IServiceCollection AddEntityFrameworkStorage<T>(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> options)
        where T : EntityFrameworkDataContext
    {
        serviceCollection.AddDbContext<T>(options);
        
        serviceCollection.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<T>());
        
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            return serviceCollection;
        
        using IServiceScope scope = serviceCollection.BuildServiceProvider().CreateScope();
        IServiceProvider serviceProvider = scope.ServiceProvider;
        
        T dataContext = serviceProvider.GetRequiredService<T>();
        
        if (dataContext.Database.GetPendingMigrations().Any())
            dataContext.Database.Migrate();
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddRedisStorage(this IServiceCollection serviceCollection)
    {
        string? redisHost = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
        
        serviceCollection.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisHost!));
        serviceCollection.AddScoped<RedisUnitOfWork>();
        
        serviceCollection.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<RedisUnitOfWork>());
        
        return serviceCollection;
    }
}