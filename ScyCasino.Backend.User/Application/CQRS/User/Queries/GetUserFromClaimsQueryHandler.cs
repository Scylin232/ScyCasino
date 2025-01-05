using System.Security.Claims;
using Application.Abstractions.Messaging;
using Application.Utilities.User;
using Domain.Repositories;
using SharedKernel.Core;

namespace Application.CQRS.User.Queries;

public sealed class GetUserFromClaimsQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserFromClaimsQuery, Domain.Models.User>
{
    public async Task<Result<Domain.Models.User>> Handle(GetUserFromClaimsQuery request, CancellationToken cancellationToken)
    {
        Claim? subjectClaim = request.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        
        if (subjectClaim is null)
            return Result.Failure<Domain.Models.User>(Error.NullValue);
        
        Guid userId = UserUtilities.GenerateGuidFromClaims(subjectClaim.Issuer, subjectClaim.Value);
        Domain.Models.User? user = await userRepository.GetById(userId);
        
        if (user is null)
            return Result.Failure<Domain.Models.User>(Error.NotFound);
        
        return Result.Success(user);
    }
}