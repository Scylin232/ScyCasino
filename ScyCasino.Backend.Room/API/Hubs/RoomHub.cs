using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;using Application.CQRS.Room.Commands;
using Application.Events;
using Domain.Models;
using Shared.Kernel.Core;

namespace API.Hubs;

public class RoomHub(ISender sender) : Hub
{
    public override async Task OnConnectedAsync()
    {
        string? accessToken = Context.GetHttpContext()?.Request.Query["access_token"];
        string? roomId = Context.GetHttpContext()?.Request.Query["roomId"];
        
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(roomId)) return;
        
        JoinUserToRoomCommand command = new(Guid.Parse(roomId), accessToken, Context.ConnectionId);
        Result<Room> result = await sender.Send(command);
        
        if (result.IsFailure) return;
        
        Room updatedRoom = result.Value;
        
        await Groups.AddToGroupAsync(Context.ConnectionId, updatedRoom.Id.ToString());
        
        await Clients
            .Group(updatedRoom.Id.ToString())
            .SendAsync(EventNames.PlayerListUpdated, updatedRoom.PlayerConnections.Values);
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        DisconnectUserFromAllRoomsByConnectionIdCommand command = new(Context.ConnectionId);
        Result<IEnumerable<Room>> result = await sender.Send(command);
        
        if (result.IsFailure) return;
        
        foreach (Room affectedRoom in result.Value)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, affectedRoom.Id.ToString());
            
            await Clients
                .Group(affectedRoom.Id.ToString())
                .SendAsync(EventNames.PlayerListUpdated, affectedRoom.PlayerConnections.Values);
        }
    }
}