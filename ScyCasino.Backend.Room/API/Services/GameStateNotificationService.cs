using API.Hubs;
using Application.Events;
using Domain.Services;
using Microsoft.AspNetCore.SignalR;

namespace API.Services;

public class GameStateNotificationService(IHubContext<RoomHub> roomHubContext) : IGameStateNotificationService
{
    public async Task NotifyGameStateUpdate(string roomId, string message) =>
        await SendMethodToGroup(roomId, EventNames.GameStateUpdated, message);
    
    public async Task NotifyGameStateUpdate(IEnumerable<string> roomIds, string message) =>
        await SendMethodToGroups(roomIds, EventNames.GameStateUpdated, message);
    
    public async Task NotifyRoundEnd(IEnumerable<string> roomIds, string message) =>
        await SendMethodToGroups(roomIds, EventNames.RoundEnded, message);
    
    private async Task SendMethodToGroup(string group, string method, string message)
    {
        await roomHubContext.Clients.Group(group).SendAsync(method, message);
    }
    
    private async Task SendMethodToGroups(IEnumerable<string> groups, string method, string message)
    {
        await roomHubContext.Clients.Groups(groups).SendAsync(method, message);
    }
}