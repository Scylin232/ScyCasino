using Domain.Repositories;
using Domain.Services;
using MassTransit;
using Shared.Application.Events.Roulette;
using Shared.Domain.Models.Room;

namespace Application.Consumers.Roulette;

public class RouletteBetsCollectedEventConsumer(IGameStateNotificationService gameStateNotificationService, IRoomRepository roomRepository) : IConsumer<RouletteBetsCollectedEvent>
{
    public async Task Consume(ConsumeContext<RouletteBetsCollectedEvent> context)
    {
        IEnumerable<string> rouletteRoomIds = (await roomRepository.GetAllRoomsOfType(RoomType.RouletteRoom))
            .Select(rouletteRoom => rouletteRoom.Id.ToString());
        
        await gameStateNotificationService.NotifyRoundEnd(rouletteRoomIds, $"{{ \"winningNumber\": {context.Message.WinningNumber} }}");
    }
}