using Microsoft.AspNetCore.SignalR;
using Application.Abstractions.Messaging;
using Application.Events;
using Domain.Repositories;
using Domain.Services;
using SharedKernel.Core;
using SharedKernel.Repositories;

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
        
        await roomRepository.AddPlayer(room, userId.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(room);
    }
}