using Domain.Models;
using Shared.Kernel.Repositories;

namespace Domain.Repositories;

public interface IRouletteBetsRepository : IGenericRepository<RouletteBet>
{
    Task<List<RouletteBet>> GetRouletteBetsByIds(List<Guid> ids);
    Task<List<RouletteBet>> GetUnhandledBets();
};