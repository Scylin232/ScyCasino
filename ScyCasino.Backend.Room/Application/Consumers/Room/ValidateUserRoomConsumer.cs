using Domain.Repositories;
using MassTransit;
using Shared.Application.Contracts.Requests.Room;

namespace Application.Consumers.Room;

public class ValidateUserRoomConsumer(IRoomRepository roomRepository) : IConsumer<ValidateUserRoomRequest>
{
    public async Task Consume(ConsumeContext<ValidateUserRoomRequest> context)
    {
        Guid roomId = context.Message.RoomId;
        Domain.Models.Room? room = await roomRepository.GetById(roomId);
        
        if (room is null)
        {
            await context.RespondAsync(new ValidateUserRoomResponse { IsValid = false});
            return;
        }
        
        Guid userId = context.Message.UserId;
        
        if (!room.PlayerConnections.ContainsValue(userId))
        {
            await context.RespondAsync(new ValidateUserRoomResponse { IsValid = false });
            return;
        }
        
        await context.RespondAsync(new ValidateUserRoomResponse { IsValid = true });
    }
}