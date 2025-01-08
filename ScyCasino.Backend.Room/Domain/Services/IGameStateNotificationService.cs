namespace Domain.Services;

public interface IGameStateNotificationService
{
    Task NotifyGameStateUpdate(string roomId, string message);
    Task NotifyGameStateUpdate(IEnumerable<string> roomIds, string message);
    Task NotifyRoundEnd(IEnumerable<string> roomIds, string message);
}