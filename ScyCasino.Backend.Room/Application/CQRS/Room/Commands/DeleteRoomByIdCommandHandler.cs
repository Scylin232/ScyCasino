using Shared.Application.Abstractions.Messaging;
using Domain.Repositories;
using MassTransit;
using Shared.Application.Events.Room;
using Shared.Kernel.Core;
using Shared.Kernel.Repositories;

namespace Application.CQRS.Room.Commands;

public sealed class DeleteRoomByIdCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork, IBus bus) : ICommandHandler<DeleteRoomByIdCommand>
{
    public async Task<Result> Handle(DeleteRoomByIdCommand request, CancellationToken cancellationToken)
    {
        Domain.Models.Room? room = await roomRepository.GetById(request.Id);
        
        if (room is null)
            return Result.Failure(Error.NotFound);
        
        await roomRepository.Remove(room);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        await bus.Publish(new RoomDeletedEvent
        {
            RoomId = room.Id,
            RoomType = room.RoomType
        }, cancellationToken);
        
        return Result.Success();
    }
}