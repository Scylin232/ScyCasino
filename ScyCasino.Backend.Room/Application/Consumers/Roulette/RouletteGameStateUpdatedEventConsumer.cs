using System.Text.Json;
using Domain.Services;
using MassTransit;
using Shared.Application.Events.Roulette;

namespace Application.Consumers.Roulette;

public class RouletteGameStateUpdatedEventConsumer(IGameStateNotificationService gameStateNotificationService) : IConsumer<RouletteGameStateUpdatedEvent>
{
    public async Task Consume(ConsumeContext<RouletteGameStateUpdatedEvent> context)
    {
        string serializedGameState = JsonSerializer.Serialize(context.Message.PlacedBets);
        await gameStateNotificationService.NotifyGameStateUpdate(context.Message.RoomId.ToString(), serializedGameState);
    }
}