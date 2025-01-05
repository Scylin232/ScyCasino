using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Repositories;
using SharedKernel.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;

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
        serviceCollection.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING_DEVELOPMENT"));
        });
        
        serviceCollection.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<DataContext>());
        
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            return serviceCollection;
        
        using IServiceScope scope = serviceCollection.BuildServiceProvider().CreateScope();
        IServiceProvider serviceProvider = scope.ServiceProvider;
        
        DataContext dataContext = serviceProvider.GetRequiredService<DataContext>();
        
        if (dataContext.Database.GetPendingMigrations().Any())
            dataContext.Database.Migrate();
        
        return serviceCollection;
    }
}