using Domain.Repositories;
using Shared.Application.Abstractions.Messaging;
using Shared.Kernel.Core;

namespace Application.CQRS.Room.Queries;

public sealed class GetAllRoomsQueryHandler(IRoomRepository roomRepository) : IQueryHandler<GetAllRoomsQuery, PaginatedResult<Domain.Models.Room>>
{
    public async Task<Result<PaginatedResult<Domain.Models.Room>>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        if (request.Context.Page <= 0 || request.Context.Count is <= 0 or > 100)
            return Result.Failure<PaginatedResult<Domain.Models.Room>>(Error.ConditionNotMet);
        
        PaginatedResult<Domain.Models.Room> rooms = await roomRepository.GetAll(request.Context);
        
        return Result.Success(rooms);
    }
}