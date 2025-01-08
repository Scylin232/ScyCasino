namespace Shared.Application.Events.Roulette;

public class RouletteBetCreatedEvent
{
    public Guid RoomId { get; set; }
    public Guid BetId { get; set; }
}