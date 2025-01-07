using Domain.Models;
using Shared.Kernel.Repositories;

namespace Domain.Repositories;

public interface IRoomRepository : IGenericRepository<Room>
{
    Task AddPlayerConnection(Room room, Guid userId, string connectionId);
    Task RemovePlayerConnection(Room room, string connectionId);
    Task<IEnumerable<Room>> RemovePlayerConnectionFromAllRooms(string connectionId);
};