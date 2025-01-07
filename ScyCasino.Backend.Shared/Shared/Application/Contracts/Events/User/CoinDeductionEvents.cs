namespace Shared.Application.Contracts.Events.User;

public interface ICoinsDeductedEvent
{
    Guid CorrelationId { get; }
    Guid UserId { get; }
    decimal Amount { get; }
}

public interface ICoinsDeductFailedEvent
{
    Guid CorrelationId { get; }
    string Reason { get; }
}