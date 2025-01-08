using Domain.Repositories;
using MassTransit;
using Shared.Application.Contracts.Requests.Roulette;
using Shared.Application.Events.Roulette;
using Shared.Kernel.Repositories;

namespace Application.Activities.Roulette;

public class ClearRouletteGameStatesActivity(IRouletteGameStatesRepository rouletteGameStatesRepository, IUnitOfWork unitOfWork, IBus bus) : IExecuteActivity<ClearRouletteGameStatesContract>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<ClearRouletteGameStatesContract> execution)
    {
        await rouletteGameStatesRepository.ClearAllGameStates();
        await unitOfWork.SaveChangesAsync();
        
        await bus.Publish(new RouletteGameStateClearedEvent());
        
        return execution.Completed();
    }
}