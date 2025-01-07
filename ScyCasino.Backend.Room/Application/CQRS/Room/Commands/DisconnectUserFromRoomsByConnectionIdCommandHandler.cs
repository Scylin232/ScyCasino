using Domain.Repositories;
using Shared.Application.Abstractions.Messaging;
using Shared.Kernel.Core;
using Shared.Kernel.Repositories;

namespace Application.CQRS.Room.Commands;

public sealed class DisconnectUserFromRoomsByConnectionIdCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork) : ICommandHandler<DisconnectUserFromAllRoomsByConnectionIdCommand, IEnumerable<Domain.Models.Room>>
{
    public async Task<Result<IEnumerable<Domain.Models.Room>>> Handle(DisconnectUserFromAllRoomsByConnectionIdCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Models.Room> affectedRooms =
            await roomRepository.RemovePlayerConnectionFromAllRooms(request.ConnectionId);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(affectedRooms);
    }
}