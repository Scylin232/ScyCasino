using Domain.Models;
using Shared.Kernel.Repositories;

namespace Domain.Repositories;

public interface IRouletteGameStatesRepository : IGenericRepository<RouletteGameState>
{
    Task ClearAllGameStates();
}