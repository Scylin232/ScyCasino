using Shared.Application.Abstractions.Messaging;
using Domain.Repositories;
using Shared.Kernel.Core;

namespace Application.CQRS.User.Queries;

public sealed class GetUserByIdQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserByIdQuery, Domain.Models.User>
{
    public async Task<Result<Domain.Models.User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Models.User? user = await userRepository.GetById(request.Id);
        
        return user is null ? Result.Failure<Domain.Models.User>(Error.NotFound) : Result.Success(user);
    }
}