using Application.Abstractions.Messaging;
using Domain.Repositories;
using SharedKernel.Core;
using SharedKernel.Repositories;

namespace Application.CQRS.Room.Commands;

public sealed class UpdateRoomByIdCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateRoomByIdCommand>
{
    public async Task<Result> Handle(UpdateRoomByIdCommand request, CancellationToken cancellationToken)
    {
        Domain.Models.Room? room = await roomRepository.GetById(request.Id);

        if (room is null)
            return Result.Failure(Error.NotFound);

        room.Name = request.Room.Name;
        room.RoomType = request.Room.RoomType;

        await roomRepository.Update(room);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}