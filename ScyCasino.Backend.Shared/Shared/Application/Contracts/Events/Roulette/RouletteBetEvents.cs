namespace Shared.Application.Contracts.Events.Roulette;

public interface IPlaceRouletteBetEvent
{
    Guid CorrelationId { get; }
    Guid UserId { get; }
    Guid RoomId { get; }
    decimal Amount { get; }
    int BetType { get; }
    int[] BetValues { get; }
}

public interface IRouletteBetCreatedEvent
{
    Guid CorrelationId { get; }
    Guid RouletteBetId { get; }
}

public interface IRouletteBetCreationFailedEvent
{
    Guid CorrelationId { get; }
    string Reason { get; }
}