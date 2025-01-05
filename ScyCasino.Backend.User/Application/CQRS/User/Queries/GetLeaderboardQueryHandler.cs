using Application.Abstractions.Messaging;
using Domain.Repositories;
using SharedKernel.Core;

namespace Application.CQRS.User.Queries;

public sealed class GetLeaderboardQueryHandler(IUserRepository userRepository) : IQueryHandler<GetLeaderboardQuery, PaginatedResult<Domain.Models.User>>
{
    public async Task<Result<PaginatedResult<Domain.Models.User>>> Handle(GetLeaderboardQuery request, CancellationToken cancellationToken)
    {
        if (request.Context.Page <= 0 || request.Context.Count is <= 0 or > 100)
            return Result.Failure<PaginatedResult<Domain.Models.User>>(Error.ConditionNotMet);
        
        PaginatedResult<Domain.Models.User> leaderboardUsers = await userRepository.GetAll(request.Context);
        
        return Result.Success(leaderboardUsers);
    }
}