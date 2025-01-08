using Domain.Models;
using Domain.Repositories;
using Shared.Application.Abstractions.Messaging;
using Shared.Kernel.Core;

namespace Application.CQRS.Roulette.Queries;

public sealed class GetAllBetsQueryHandler(IRouletteBetsRepository rouletteBetsRepository) : IQueryHandler<GetAllBetsQuery, PaginatedResult<RouletteBet>>
{
    public async Task<Result<PaginatedResult<RouletteBet>>> Handle(GetAllBetsQuery request, CancellationToken cancellationToken)
    {
        return Result.Success(await rouletteBetsRepository.GetAll(request.Context));
    }
}