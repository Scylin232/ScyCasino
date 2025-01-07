using Shared.Application.Abstractions.Messaging;
using Domain.Repositories;
using Shared.Kernel.Core;
using Shared.Kernel.Repositories;

namespace Application.CQRS.Room.Commands;

public sealed class CreateRoomCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateRoomCommand>
{
    public async Task<Result> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        Domain.Models.Room newRoom = new(Guid.NewGuid())
        {
            Name = request.Room.Name,
            RoomType = request.Room.RoomType
        };
        await roomRepository.Add(newRoom);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}