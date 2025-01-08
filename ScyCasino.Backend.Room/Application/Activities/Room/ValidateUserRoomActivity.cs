using Domain.Repositories;
using MassTransit;
using Shared.Application.Contracts.Requests.Room;

namespace Application.Activities.Room;

public class ValidateUserRoomActivity(IRoomRepository roomRepository) : IExecuteActivity<ValidateUserRoomContract>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<ValidateUserRoomContract> execution)
    {
        Guid roomId = execution.Arguments.RoomId;
        Domain.Models.Room? room = await roomRepository.GetById(roomId);
        
        Guid? userId = execution.GetVariable<Guid>("UserId");
        
        if (room is null || userId is null || !room.PlayerConnections.ContainsValue(userId.Value))
            return execution.Faulted();
        
        return execution.Completed();
    }
}