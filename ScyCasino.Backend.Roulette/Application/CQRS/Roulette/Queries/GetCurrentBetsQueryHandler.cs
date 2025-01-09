using Domain.Models;
using Domain.Repositories;
using Shared.Application.Abstractions.Messaging;
using Shared.Application.Events.Roulette;
using Shared.Kernel.Core;

namespace Application.CQRS.Roulette.Queries;

public sealed class GetCurrentBetsQueryHandler(IRouletteGameStatesRepository gameStatesRepository, IRouletteBetsRepository betsRepository) : IQueryHandler<GetCurrentBetsQuery, List<PlacedRouletteBet>>
{
    public async Task<Result<List<PlacedRouletteBet>>> Handle(GetCurrentBetsQuery request, CancellationToken cancellationToken)
    {
        RouletteGameState? gameState = await gameStatesRepository.GetById(request.RoomId);
        
        if (gameState is null)
            return Result.Failure<List<PlacedRouletteBet>>(Error.NotFound);
        
        List<PlacedRouletteBet> placedBets = (await betsRepository.GetRouletteBetsByIds(gameState.PlacedBets))
            .Select(bet => new PlacedRouletteBet
            {
                UserId = bet.UserId,
                Amount = bet.Amount,
                RouletteBetType = (int)bet.BetType,
                BetValues = bet.BetValues
            }).ToList();
        
        return Result.Success(placedBets);
    }
}