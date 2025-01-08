using Domain.Models;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Repositories;

namespace Infrastructure.Repositories;

public class RouletteGameStatesRepository(DataContext dataContext) : EntityFrameworkRepository<RouletteGameState>(dataContext), IRouletteGameStatesRepository
{
    public async Task ClearAllGameStates()
    {
        List<RouletteGameState> gameStates = await dataContext.RouletteGameStates.ToListAsync();
        
        foreach (RouletteGameState gameState in gameStates)
        {
            gameState.PlacedBets.Clear();
            await Update(gameState);
        }
    }
}