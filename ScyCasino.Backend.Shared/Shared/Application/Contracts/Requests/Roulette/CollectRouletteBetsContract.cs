namespace Shared.Application.Contracts.Requests.Roulette;

public class CollectRouletteBetsContract
{
    public int WinningNumber { get; set; }
}

public class CollectRouletteBetsLog
{
    public List<Guid> HandledBets { get; set; } = new();
}