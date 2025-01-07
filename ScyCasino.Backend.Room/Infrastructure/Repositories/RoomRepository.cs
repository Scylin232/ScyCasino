using System.Text.Json;
using Domain.Models;
using Domain.Repositories;
using Shared.Infrastructure.Data;
using Shared.Infrastructure.Repositories;
using StackExchange.Redis;

namespace Infrastructure.Repositories;

public class RoomRepository(RedisUnitOfWork unitOfWork) : RedisRepository<Room>(unitOfWork), IRoomRepository
{
    private readonly RedisUnitOfWork _unitOfWork = unitOfWork;

    public async Task AddPlayerConnection(Room room, Guid userId, string connectionId)
    {
        room.PlayerConnections.Add(connectionId, userId);
        
        await Update(room);
    }
    
    public async Task RemovePlayerConnection(Room room, string connectionId)
    {
        room.PlayerConnections.Remove(connectionId);
        
        await Update(room);
    }
    
    public async Task<IEnumerable<Room>> RemovePlayerConnectionFromAllRooms(string connectionId)
    {
        IDatabase database = _unitOfWork.ConnectionMultiplexer.GetDatabase();
        RedisValue[] keys = await database.SetMembersAsync(CollectionKey);
        
        List<Room> affectedRooms = new();
        
        foreach (RedisValue key in keys)
        {
            RedisValue value = await database.StringGetAsync(new RedisKey(key));
            
            if (value.IsNullOrEmpty) continue;
            
            Room? room = JsonSerializer.Deserialize<Room>(value!);
            
            if (room is null || !room.PlayerConnections.ContainsKey(connectionId)) continue;
            
            await RemovePlayerConnection(room, connectionId);
            
            affectedRooms.Add(room);
        }
        
        return affectedRooms;
    }
}