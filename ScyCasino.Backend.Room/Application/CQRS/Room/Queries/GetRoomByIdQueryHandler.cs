using Application.Abstractions.Messaging;
using Domain.Repositories;
using SharedKernel.Core;

namespace Application.CQRS.Room.Queries;

public sealed class GetRoomByIdQueryHandler(IRoomRepository roomRepository) : IQueryHandler<GetRoomByIdQuery, Domain.Models.Room>
{
    public async Task<Result<Domain.Models.Room>> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Models.Room? room = await roomRepository.GetById(request.Id);
        
        if (room is null)
            return Result.Failure<Domain.Models.Room>(Error.NotFound);
        
        return Result.Success(room);
    }
}