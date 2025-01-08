using Domain.Repositories;
using Domain.Services;
using MassTransit;
using Shared.Application.Events.Roulette;
using Shared.Domain.Models.Room;

namespace Application.Consumers.Roulette;

public class RouletteGameStateClearedEventConsumer(IGameStateNotificationService gameStateNotificationService, IRoomRepository roomRepository) : IConsumer<RouletteGameStateClearedEvent>
{
    public async Task Consume(ConsumeContext<RouletteGameStateClearedEvent> context)
    {
        IEnumerable<string> rouletteRoomIds = (await roomRepository.GetAllRoomsOfType(RoomType.RouletteRoom))
            .Select(rouletteRoom => rouletteRoom.Id.ToString());
        
        await gameStateNotificationService.NotifyGameStateUpdate(rouletteRoomIds, "[]");
    }
}