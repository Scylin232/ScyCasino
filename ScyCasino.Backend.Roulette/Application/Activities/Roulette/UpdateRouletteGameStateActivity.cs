using Domain.Models;
using Domain.Repositories;
using MassTransit;
using Shared.Application.Contracts.Requests.Roulette;
using Shared.Application.Events.Roulette;
using Shared.Kernel.Repositories;

namespace Application.Activities.Roulette;

public class UpdateRouletteGameStateActivity(IRouletteGameStatesRepository rouletteGameStatesRepository, IRouletteBetsRepository rouletteBetsRepository, IUnitOfWork unitOfWork, IBus bus) : IExecuteActivity<UpdateRouletteGameStateContract>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<UpdateRouletteGameStateContract> execution)
    {
        Guid roomId = execution.Arguments.RoomId;
        Guid? betId = execution.GetVariable<Guid>("BetId");
        
        if (betId is null)
            return execution.Faulted();
        
        RouletteGameState? gameState = await rouletteGameStatesRepository.GetById(roomId);
        
        if (gameState is null)
            return execution.Faulted();
        
        gameState.PlacedBets.Add(betId.Value);
        await rouletteGameStatesRepository.Update(gameState);
        await unitOfWork.SaveChangesAsync();
        
        List<RouletteBet> placedBets = await rouletteBetsRepository.GetRouletteBetsByIds(gameState.PlacedBets);
        
        await bus.Publish(new RouletteGameStateUpdatedEvent
        {
            RoomId = roomId,
            PlacedBets = placedBets.Select(bet => new PlacedRouletteBet
            {
                UserId = bet.UserId,
                Amount = bet.Amount,
                RouletteBetType = (int)bet.BetType,
                BetValues = bet.BetValues
            }).ToList()
        });
        
        return execution.Completed();
    }
}