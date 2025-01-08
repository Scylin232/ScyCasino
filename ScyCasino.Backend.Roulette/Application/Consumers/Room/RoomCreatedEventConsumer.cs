using Domain.Models;
using Domain.Repositories;
using MassTransit;
using Shared.Application.Events.Room;
using Shared.Domain.Models.Room;
using Shared.Kernel.Repositories;

namespace Application.Consumers.Room;

public class RoomCreatedEventConsumer(IRouletteGameStatesRepository rouletteGameStatesRepository, IUnitOfWork unitOfWork) : IConsumer<RoomCreatedEvent>
{
    public async Task Consume(ConsumeContext<RoomCreatedEvent> context)
    {
        if (context.Message.RoomType != RoomType.RouletteRoom) return;
        
        RouletteGameState newGameState = new(context.Message.RoomId);
        
        await rouletteGameStatesRepository.Add(newGameState);
        await unitOfWork.SaveChangesAsync();
    }
}