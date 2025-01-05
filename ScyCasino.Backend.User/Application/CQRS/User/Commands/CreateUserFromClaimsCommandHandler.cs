using System.Security.Claims;
using Application.Abstractions.Messaging;
using Application.Utilities.User;
using Domain.Repositories;
using SharedKernel.Core;
using SharedKernel.Repositories;

namespace Application.CQRS.User.Commands;

public sealed class CreateUserFromClaimsCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateUserFromClaimsCommand>
{
    public async Task<Result> Handle(CreateUserFromClaimsCommand request, CancellationToken cancellationToken)
    {
        Claim? subjectClaim = request.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        
        if (subjectClaim is null)
            return Result.Failure(Error.NullValue);
        
        Guid userId = UserUtilities.GenerateGuidFromClaims(subjectClaim.Issuer, subjectClaim.Value);
        Domain.Models.User? existingUser = await userRepository.GetById(userId);
        
        if (existingUser is not null)
            return Result.Success();
        
        Claim? nicknameClaim = request.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName);
        
        if (nicknameClaim is null)
            return Result.Failure(Error.NullValue);
        
        Domain.Models.User newUser = new(userId)
        {
            Coins = 500,
            Nickname = nicknameClaim.Value
        };
        await userRepository.Add(newUser);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}