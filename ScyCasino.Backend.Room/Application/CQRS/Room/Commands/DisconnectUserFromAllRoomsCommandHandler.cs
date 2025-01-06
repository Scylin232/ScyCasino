using System.Collections;
using Application.Abstractions.Messaging;
using Domain.Repositories;
using Domain.Services;
using SharedKernel.Core;
using SharedKernel.Repositories;

namespace Application.CQRS.Room.Commands;

public sealed class DisconnectUserFromAllRoomsCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork, IUserService userService) : ICommandHandler<DisconnectUserFromAllRoomsCommand, IEnumerable<Domain.Models.Room>>
{
    public async Task<Result<IEnumerable<Domain.Models.Room>>> Handle(DisconnectUserFromAllRoomsCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = await userService.GetUserIdByToken(request.UserToken);
        
        if (userId is null)
            return Result.Failure<IEnumerable<Domain.Models.Room>>(Error.ConditionNotMet);
        
        IEnumerable<Domain.Models.Room> rooms = await roomRepository.RemovePlayerFromAllRooms(userId.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(rooms);
    }
}