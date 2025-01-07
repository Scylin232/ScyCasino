using Shared.Kernel.Domain;

namespace Domain.Models;

public class RouletteBet(Guid id) : Entity(id)
{
    public Guid UserId { get; set; }
    public int Amount { get; set; }
    
    public RouletteBetType BetType { get; set; }
    public int[] BetValues { get; set; }
    
    public bool IsWinner { get; set; }
}