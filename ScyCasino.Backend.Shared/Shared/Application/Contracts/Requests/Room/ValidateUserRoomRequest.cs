namespace Shared.Application.Contracts.Requests.Room;

public class ValidateUserRoomRequest
{
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
}

public class ValidateUserRoomResponse
{
    public bool IsValid { get; set; }
}