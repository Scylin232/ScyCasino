using Domain.Repositories;
using MassTransit;
using Shared.Application.Contracts.Requests.User;
using Shared.Kernel.Repositories;

namespace Application.Activities.User;

public class ConsumeCoinsActivity(IUserRepository userRepository, IUnitOfWork unitOfWork) : IActivity<ConsumeCoinsContract, ConsumeCoinsLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<ConsumeCoinsContract> execution)
    {
        Guid? userId = execution.GetVariable<Guid>("UserId");
        
        if (userId is null)
            return execution.Faulted();
        
        Domain.Models.User? user = await userRepository.GetById(userId.Value);
        
        if (user is null || execution.Arguments.Amount > user.Coins)
            return execution.Faulted();
        
        user.Coins -= execution.Arguments.Amount;
        await unitOfWork.SaveChangesAsync();
        
        return execution.CompletedWithVariables(new ConsumeCoinsLog
        {
            ConsumedUserId = user.Id, 
            ConsumedAmount = execution.Arguments.Amount
        }, new
        {
            execution.Arguments.Amount
        });
    }
    
    public async Task<CompensationResult> Compensate(CompensateContext<ConsumeCoinsLog> execution)
    {
        Guid userId = execution.Log.ConsumedUserId;
        Domain.Models.User? user = await userRepository.GetById(userId);
        
        if (user is null)
            return execution.Failed();
        
        user.Coins += execution.Log.ConsumedAmount;
        await unitOfWork.SaveChangesAsync();
        
        return execution.Compensated();
    }
}