using Shared.Kernel.Domain;

namespace Domain.Models;

public class RouletteGameState(Guid id) : Entity(id)
{
    public List<Guid> PlacedBets { get; set; } = [];
}