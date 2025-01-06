using Domain.Models;
using SharedKernel.Repositories;

namespace Domain.Repositories;

public interface IRoomRepository : IGenericRepository<Room>
{
    Task AddPlayer(Room room, Guid userId);
    Task RemovePlayer(Room room, Guid userId);
    Task<IEnumerable<Room>> RemovePlayerFromAllRooms(Guid userId);
};