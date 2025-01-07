using System.Text.Json;
using Shared.Infrastructure.Data;
using Shared.Kernel.Core;
using Shared.Kernel.Domain;
using Shared.Kernel.Repositories;
using StackExchange.Redis;

namespace Shared.Infrastructure.Repositories;

public class RedisRepository<T>(RedisUnitOfWork unitOfWork) : IGenericRepository<T>
    where T : Entity
{
    protected readonly string CollectionKey = typeof(T).Name;
    
    public async Task<PaginatedResult<T>> GetAll(GetAllContext context)
    {
        IDatabase database = unitOfWork.ConnectionMultiplexer.GetDatabase();
        RedisValue[] keys = await database.SetMembersAsync(CollectionKey);
        
        List<T> entities = new();
        
        foreach (RedisValue key in keys.Skip((context.Page - 1) * context.Count).Take(context.Count))
        {
            RedisValue value = await database.StringGetAsync(new RedisKey(key));
            
            if (value.IsNullOrEmpty) continue;
            
            T? entity = JsonSerializer.Deserialize<T>(value!);
            
            if (entity is null) continue;
            
            entities.Add(entity);
        }
        
        int totalCount = (int)await database.SetLengthAsync(CollectionKey);
        int totalPages = (int)Math.Ceiling((double)totalCount / context.Count);
        
        return new PaginatedResult<T>
        {
            TotalCount = totalCount,
            TotalPages = totalPages,
            Results = entities
        };
    }
    
    public async Task<T?> GetById(Guid id)
    {
        IDatabase database = unitOfWork.ConnectionMultiplexer.GetDatabase();
        
        string key = GetEntityKey(id);
        RedisValue value = await database.StringGetAsync(key);
        
        return value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(value!);
    }
    
    public Task Add(T newEntity)
    {
        string key = GetEntityKey(newEntity);
        string serializedEntity = JsonSerializer.Serialize(newEntity);
        
        unitOfWork.RegisterOperation(async db =>
        {
            await db.StringSetAsync(key, serializedEntity);
            await db.SetAddAsync(CollectionKey, key);
        });
        
        return Task.CompletedTask;
    }
    
    public Task Update(T updatedEntity)
    {
        updatedEntity.UpdatedAt = DateTime.UtcNow;
        
        string key = GetEntityKey(updatedEntity);
        string serializedEntity = JsonSerializer.Serialize(updatedEntity);
        
        unitOfWork.RegisterOperation(async db =>
        {
            await db.StringSetAsync(key, serializedEntity);
        });
        
        return Task.CompletedTask;
    }
    
    public Task Remove(T deletedEntity)
    {
        string key = GetEntityKey(deletedEntity);
        
        unitOfWork.RegisterOperation(async db =>
        {
            await db.KeyDeleteAsync(key);
            await db.SetRemoveAsync(CollectionKey, key);
        });

        return Task.CompletedTask;
    }
    
    protected string GetEntityKey(Guid id) => $"{CollectionKey}:{id}";
    protected string GetEntityKey(T entity) => GetEntityKey(entity.Id);
}