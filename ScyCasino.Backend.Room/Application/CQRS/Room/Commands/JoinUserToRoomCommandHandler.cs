using Shared.Application.Abstractions.Messaging;
using Domain.Repositories;
using Domain.Services;
using Shared.Kernel.Core;
using Shared.Kernel.Repositories;

namespace Application.CQRS.Room.Commands;

public sealed class JoinUserToRoomCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork, IUserService userService) : ICommandHandler<JoinUserToRoomCommand, Domain.Models.Room>
{
    public async Task<Result<Domain.Models.Room>> Handle(JoinUserToRoomCommand request, CancellationToken cancellationToken)
    {
        Domain.Models.Room? room = await roomRepository.GetById(request.RoomId);
        
        if (room is null)
            return Result.Failure<Domain.Models.Room>(Error.NotFound);
        
        Guid? userId = await userService.GetUserIdByToken(request.UserToken);
        
        if (userId is null)
            return Result.Failure<Domain.Models.Room>(Error.NotFound);
        
        if (room.PlayerConnections.ContainsValue(userId.Value) || room.PlayerConnections.ContainsKey(request.ConnectionId))
            return Result.Failure<Domain.Models.Room>(Error.ConditionNotMet);
        
        await roomRepository.AddPlayerConnection(room, userId.Value, request.ConnectionId);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(room);
    }
}