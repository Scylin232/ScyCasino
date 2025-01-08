using Shared.Domain.Models.Room;

namespace Shared.Application.Events.Room;

public class RoomCreatedEvent
{
    public Guid RoomId { get; set; }
    public RoomType RoomType { get; set; }
}