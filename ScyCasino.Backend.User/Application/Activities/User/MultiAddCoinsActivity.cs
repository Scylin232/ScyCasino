using Domain.Repositories;
using MassTransit;
using Shared.Application.Contracts.Requests.User;
using Shared.Kernel.Repositories;

namespace Application.Activities.User;

public class MultiAddCoinsActivity(IUserRepository userRepository, IUnitOfWork unitOfWork) : IActivity<MultiAddCoinsContract, MultiAddCoinsLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<MultiAddCoinsContract> execution)
    {
        Dictionary<Guid, int>? userAmount = execution.GetVariable<Dictionary<Guid, int>>("UserAmount");
        
        if (userAmount is null)
            return execution.Faulted();
        
        List<Domain.Models.User> users = await userRepository.GetUsersByIds(userAmount.Select(user => user.Key));
        
        foreach (Domain.Models.User user in users)
        {
            user.Coins += userAmount[user.Id];
            await userRepository.Update(user);
        }
        
        await unitOfWork.SaveChangesAsync();
        
        return execution.Completed(new MultiAddCoinsLog
        {
            CollectedUserAmount = userAmount
        });
    }
    
    public async Task<CompensationResult> Compensate(CompensateContext<MultiAddCoinsLog> execution)
    {
        Dictionary<Guid, int> collectedUserAmount = execution.Log.CollectedUserAmount;
        List<Domain.Models.User> users = await userRepository.GetUsersByIds(collectedUserAmount.Select(user => user.Key));
        
        foreach (Domain.Models.User user in users)
        {
            user.Coins -= collectedUserAmount[user.Id];
            await userRepository.Update(user);
        }
        
        await unitOfWork.SaveChangesAsync();
        
        return execution.Compensated();
    }
}