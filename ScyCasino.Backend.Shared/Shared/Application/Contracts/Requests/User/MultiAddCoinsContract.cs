namespace Shared.Application.Contracts.Requests.User;

public class MultiAddCoinsContract
{
}

public class MultiAddCoinsLog
{
    public Dictionary<Guid, int> CollectedUserAmount { get; set; } = new();
}