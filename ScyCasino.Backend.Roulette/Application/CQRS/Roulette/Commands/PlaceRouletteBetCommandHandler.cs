using Domain.Models;
using Domain.Repositories;
using MassTransit;
using Shared.Application.Abstractions.Messaging;
using Shared.Kernel.Core;
using Domain.Services;
using MassTransit.Initializers;
using Shared.Application.Events.Roulette;
using Shared.Kernel.Repositories;

namespace Application.CQRS.Roulette.Commands;

public sealed class PlaceRouletteBetCommandHandler(IRouletteGameStatesRepository rouletteGameStatesRepository, IRouletteBetsRepository rouletteBetsRepository, IUnitOfWork unitOfWork, IRouletteService rouletteService, IRequestClient<PlaceRouletteBetCommand> requestClient, IBus bus) : ICommandHandler<PlaceRouletteBetCommand>
{
    public async Task<Result> Handle(PlaceRouletteBetCommand request, CancellationToken cancellationToken)
    {
        if (!rouletteService.ValidateBet(request.BetType, request.BetValues))
            return Result.Failure(Error.ConditionNotMet);
        
        try
        {
            Response<RouletteBetCreatedEvent> response = await requestClient.GetResponse<RouletteBetCreatedEvent>(request, cancellationToken);
            RouletteGameState? gameState = await rouletteGameStatesRepository.GetById(response.Message.RoomId);
            
            if (gameState is null)
                throw new Exception(Error.NotFound.Message);
            
            gameState.PlacedBets.Add(response.Message.BetId);
            await rouletteGameStatesRepository.Update(gameState);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            List<RouletteBet> placedBets = await rouletteBetsRepository.GetRouletteBetsByIds(gameState.PlacedBets);
            
            await bus.Publish(new RouletteGameStateUpdatedEvent
            {
                RoomId = response.Message.RoomId,
                PlacedBets = placedBets.Select(bet => new PlacedRouletteBet
                {
                    UserId = bet.UserId,
                    Amount = bet.Amount,
                    RouletteBetType = (int)bet.BetType,
                    BetValues = bet.BetValues
                }).ToList()
            }, cancellationToken);
            
            return Result.Success();
        }
        catch
        {
            return Result.Failure(Error.ConditionNotMet);
        }
    }
}