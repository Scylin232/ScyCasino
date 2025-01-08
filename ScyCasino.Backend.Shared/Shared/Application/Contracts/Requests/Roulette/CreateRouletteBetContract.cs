namespace Shared.Application.Contracts.Requests.Roulette;

public class CreateRouletteBetContract
{
    public int BetType { get; set; }
    public int[] BetValues { get; set; }
}