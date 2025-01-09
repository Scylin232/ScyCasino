using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using MassTransit;
using Shared.Application.Contracts.Requests.Roulette;
using Shared.Application.Events.Roulette;
using Shared.Kernel.Repositories;

namespace Application.Activities.Roulette;

public class CollectRouletteBetsActivity(IRouletteBetsRepository betsRepository, IUnitOfWork unitOfWork, IRouletteService rouletteService, IBus bus) : IActivity<CollectRouletteBetsContract, CollectRouletteBetsLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<CollectRouletteBetsContract> execution)
    {
        int winningNumber = execution.Arguments.WinningNumber;
        
        List<RouletteBet> unhandledBets = await betsRepository.GetUnhandledBets();
        Dictionary<Guid, int> payout = new();
        
        foreach (RouletteBet unhandledBet in unhandledBets)
        {
            unhandledBet.Handled = true;
            
            if (unhandledBet.BetValues.Contains(winningNumber))
            {
                Guid winnerId = unhandledBet.UserId;
                int winAmount = unhandledBet.Amount * (int)rouletteService.GetBetMultiplier(unhandledBet.BetType);
                
                if (!payout.TryAdd(winnerId, winAmount))
                    payout[winnerId] += winAmount;
            }
            
            await betsRepository.Update(unhandledBet);
        }
        
        await unitOfWork.SaveChangesAsync();
        
        await bus.Publish(new RouletteBetsCollectedEvent
        {
            WinningNumber = winningNumber
        });
        
        return execution.CompletedWithVariables(new CollectRouletteBetsLog
        {
            HandledBets = unhandledBets.Select(bet => bet.Id).ToList()
        }, new
        {
            UserAmount = payout
        });
    }
    
    public async Task<CompensationResult> Compensate(CompensateContext<CollectRouletteBetsLog> execution)
    {
        List<Guid> handledBetsIds = execution.Log.HandledBets;
        List<RouletteBet> handledBets = await betsRepository.GetRouletteBetsByIds(handledBetsIds);

        foreach (RouletteBet handledBet in handledBets)
        {
            handledBet.Handled = false;
            await betsRepository.Update(handledBet);
        }

        await unitOfWork.SaveChangesAsync();

        return execution.Compensated();
    }
}