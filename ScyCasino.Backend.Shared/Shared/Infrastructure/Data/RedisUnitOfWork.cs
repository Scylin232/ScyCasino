using Shared.Kernel.Repositories;
using StackExchange.Redis;

namespace Shared.Infrastructure.Data;

public class RedisUnitOfWork(IConnectionMultiplexer connectionMultiplexer) : IUnitOfWork
{
    public IConnectionMultiplexer ConnectionMultiplexer { get; } = connectionMultiplexer;
    private readonly List<Func<IDatabase, Task>> _operations = new();
    
    public void RegisterOperation(Func<IDatabase, Task> operation) => _operations.Add(operation);
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IDatabase database = ConnectionMultiplexer.GetDatabase();
        
        foreach (Func<IDatabase,Task> operation in _operations)
            await operation(database);
        
        int operationCount = _operations.Count;
        _operations.Clear();
        
        return operationCount;
    }
}