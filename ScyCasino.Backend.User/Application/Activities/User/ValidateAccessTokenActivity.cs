using Application.Utilities.User;
using Domain.Repositories;
using MassTransit;
using Shared.Application.Contracts.Requests.User;

namespace Application.Activities.User;

public class ValidateAccessTokenActivity(IUserRepository userRepository) : IExecuteActivity<ValidateAccessTokenContract>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<ValidateAccessTokenContract> execution)
    {
        string issuer = execution.Arguments.Issuer;
        string subject = execution.Arguments.Subject;
        
        Guid userId = UserUtilities.GenerateGuidFromClaims(issuer, subject);
        
        Domain.Models.User? user = await userRepository.GetById(userId);
        
        return user is not null ? execution.CompletedWithVariables(new { UserId = userId }) : execution.Faulted();
    }
}