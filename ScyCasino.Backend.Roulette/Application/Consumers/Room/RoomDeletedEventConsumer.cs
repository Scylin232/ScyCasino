using Domain.Models;
using Domain.Repositories;
using MassTransit;
using Shared.Application.Events.Room;
using Shared.Domain.Models.Room;
using Shared.Kernel.Repositories;

namespace Application.Consumers.Room;

public class RoomDeletedEventConsumer(IRouletteGameStatesRepository rouletteGameStatesRepository, IUnitOfWork unitOfWork) : IConsumer<RoomDeletedEvent>
{
    public async Task Consume(ConsumeContext<RoomDeletedEvent> context)
    {
        if (context.Message.RoomType != RoomType.RouletteRoom) return;
        
        RouletteGameState? gameState = await rouletteGameStatesRepository.GetById(context.Message.RoomId);
        
        if (gameState is null) return;
        
        await rouletteGameStatesRepository.Remove(gameState);
        await unitOfWork.SaveChangesAsync();
    }
}