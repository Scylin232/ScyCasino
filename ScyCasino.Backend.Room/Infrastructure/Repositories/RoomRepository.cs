using System.Text.Json;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Data;
using StackExchange.Redis;

namespace Infrastructure.Repositories;

public class RoomRepository(RedisUnitOfWork unitOfWork) : RedisRepository<Room>(unitOfWork), IRoomRepository
{
    private readonly RedisUnitOfWork _unitOfWork = unitOfWork;

    public async Task AddPlayer(Room room, Guid userId)
    {
        room.Players.Add(userId);
        
        await Update(room);
    }
    
    public async Task RemovePlayer(Room room, Guid userId)
    {
        room.Players.Remove(userId);
        
        await Update(room);
    }
    
    public async Task<IEnumerable<Room>> RemovePlayerFromAllRooms(Guid userId)
    {
        IDatabase database = _unitOfWork.ConnectionMultiplexer.GetDatabase();
        
        RedisValue[] keys = await database.SetMembersAsync(CollectionKey);
        List<Room> roomsWithPlayer = new();
        
        foreach (RedisValue key in keys)
        {
            RedisValue value = await database.StringGetAsync(new RedisKey(key));
            
            if (value.IsNullOrEmpty) continue;
            
            Room? room = JsonSerializer.Deserialize<Room>(value!);
            
            if (room is null || !room.Players.Contains(userId)) continue;
            
            await RemovePlayer(room, userId);
            
            roomsWithPlayer.Add(room);
        }
        
        return roomsWithPlayer;
    }
}