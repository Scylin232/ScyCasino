using Application.Utilities.User;
using Domain.Repositories;
using MassTransit;
using Shared.Application.Contracts.Requests.User;

namespace Application.Consumers.User;

public class ValidateAccessTokenConsumer(IUserRepository userRepository) : IConsumer<ValidateAccessTokenRequest>
{
    public async Task Consume(ConsumeContext<ValidateAccessTokenRequest> context)
    {
        string issuer = context.Message.Issuer;
        string subject = context.Message.Subject;
        
        Guid userId = UserUtilities.GenerateGuidFromClaims(issuer, subject);
        
        Domain.Models.User? user = await userRepository.GetById(userId);
        
        await context.RespondAsync<ValidateAccessTokenResponse>(new
        {
            UserId = user?.Id,
            IsValid = user is not null
        });
    }
}