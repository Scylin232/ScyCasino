using Shared.Domain.Models.Room;

namespace Shared.Application.Events.Room;

public class RoomDeletedEvent
{
    public Guid RoomId { get; set; }
    public RoomType RoomType { get; set; }
}