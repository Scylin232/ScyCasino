namespace Shared.Application.Contracts.Requests.User;

public class ConsumeCoinsContract
{
    public int Amount { get; set; }
}

public class ConsumeCoinsLog
{
    public Guid ConsumedUserId { get; set; }
    public int ConsumedAmount { get; set; }
}