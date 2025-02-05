﻿using Application.Activities.Roulette;
using Application.Consumers.Room;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Contracts.Requests.Roulette;
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
            options.AddConsumer<RoomCreatedEventConsumer>();
            options.AddConsumer<RoomDeletedEventConsumer>();
            
            options.AddActivity<CollectRouletteBetsActivity, CollectRouletteBetsContract, CollectRouletteBetsLog>();
            options.AddExecuteActivity<ClearRouletteGameStatesActivity, ClearRouletteGameStatesContract>();
            options.AddExecuteActivity<CreateRouletteBetActivity, CreateRouletteBetContract>();
            options.AddExecuteActivity<UpdateRouletteGameStateActivity, UpdateRouletteGameStateContract>();
        });
        
        serviceCollection.AddScoped<IRouletteBetsRepository, RouletteBetsRepository>();
        serviceCollection.AddScoped<IRouletteGameStatesRepository, RouletteGameStatesRepository>();
        
        serviceCollection.AddSingleton<IRouletteService, RouletteService>();
        
        return serviceCollection;
    }
    
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseAuthentication();
        applicationBuilder.UseAuthorization();
        
        return applicationBuilder;
    }
}