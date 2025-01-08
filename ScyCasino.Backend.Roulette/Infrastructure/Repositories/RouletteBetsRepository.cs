using Domain.Models;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Repositories;

namespace Infrastructure.Repositories;

public class RouletteBetsRepository(DataContext dataContext) : EntityFrameworkRepository<RouletteBet>(dataContext), IRouletteBetsRepository
{
    public async Task<List<RouletteBet>> GetRouletteBetsByIds(List<Guid> ids)
    {
        IQueryable<RouletteBet> rouletteBetsQuery = dataContext.Set<RouletteBet>().AsNoTracking();
        
        return await rouletteBetsQuery
            .Where(bet => ids.Contains(bet.Id))
            .ToListAsync();
    }

    public async Task<List<RouletteBet>> GetUnhandledBets()
    {
        IQueryable<RouletteBet> unhandledBetsQuery = dataContext.Set<RouletteBet>().AsNoTracking();

        return await unhandledBetsQuery
            .Where(bet => bet.Handled == false)
            .ToListAsync();
    }
}