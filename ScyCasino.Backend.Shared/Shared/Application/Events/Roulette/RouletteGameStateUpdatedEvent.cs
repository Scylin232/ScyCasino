namespace Shared.Application.Events.Roulette;

public class PlacedRouletteBet
{
    public Guid UserId { get; set; }
    public int Amount { get; set; }
    public int RouletteBetType { get; set; }
    public int[] BetValues { get; set; }
}

public class RouletteGameStateUpdatedEvent
{
    public Guid RoomId { get; set; }
    public List<PlacedRouletteBet> PlacedBets { get; set; } = new();
}