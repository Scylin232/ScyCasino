using Domain.Models;
using Domain.Repositories;
using Shared.Application.Abstractions.Messaging;
using Shared.Kernel.Core;

namespace Application.CQRS.Roulette.Queries;

public sealed class GetAllGameStatesQueryHandler(IRouletteGameStatesRepository rouletteGameStatesRepository) : IQueryHandler<GetAllGameStatesQuery, PaginatedResult<RouletteGameState>>
{
    public async Task<Result<PaginatedResult<RouletteGameState>>> Handle(GetAllGameStatesQuery request, CancellationToken cancellationToken)
    {
        return Result.Success(await rouletteGameStatesRepository.GetAll(request.Context));
;    }
}