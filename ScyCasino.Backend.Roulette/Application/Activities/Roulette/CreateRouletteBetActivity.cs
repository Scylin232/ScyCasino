using Domain.Models;
using Domain.Repositories;
using MassTransit;
using Shared.Application.Contracts.Requests.Roulette;
using Shared.Kernel.Repositories;

namespace Application.Activities.Roulette;

public class CreateRouletteBetActivity(IRouletteBetsRepository rouletteBetsRepository, IUnitOfWork unitOfWork) : IExecuteActivity<CreateRouletteBetContract>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<CreateRouletteBetContract> execution)
    {
        Guid? userId = execution.GetVariable<Guid>("UserId");
        int? amount = execution.GetVariable<int>("Amount");
        
        if (userId is null || amount is null)
            return execution.Faulted();
        
        RouletteBet rouletteBet = new(Guid.NewGuid())
        {
            UserId = userId.Value,
            Amount = amount.Value,
            BetType = (RouletteBetType)execution.Arguments.BetType,
            BetValues = execution.Arguments.BetValues,
            IsWinner = false,
            Handled = false
        };
        await rouletteBetsRepository.Add(rouletteBet);
        await unitOfWork.SaveChangesAsync();
        
        return execution.CompletedWithVariables(new
        {
            BetId = rouletteBet.Id
        });
    }
}