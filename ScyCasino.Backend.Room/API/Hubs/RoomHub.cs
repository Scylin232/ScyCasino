using System.Collections;
using Application.CQRS.Room.Commands;
using Application.Events;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SharedKernel.Core;

namespace API.Hubs;

public class RoomHub(ISender sender) : Hub
{
    [Authorize]
    public async Task JoinRoom(Guid roomId)
    {
        string? accessToken = Context.GetHttpContext()?.Request.Query["access_token"];
        
        if (string.IsNullOrEmpty(accessToken)) return;
        
        JoinUserToRoomCommand command = new(roomId, accessToken);
        Result<Room> result = await sender.Send(command);
        
        if (result.IsFailure) return;
        
        Room updatedRoom = result.Value;
        
        await Groups.AddToGroupAsync(Context.ConnectionId, updatedRoom.Id.ToString());
        
        await Clients
            .Group(updatedRoom.Id.ToString())
            .SendAsync(EventNames.PlayerListUpdated, updatedRoom.Players);
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        string? accessToken = Context.GetHttpContext()?.Request.Query["access_token"];
        
        if (string.IsNullOrEmpty(accessToken)) return;
        
        DisconnectUserFromAllRoomsCommand command = new(accessToken);
        Result<IEnumerable<Room>> result = await sender.Send(command);
        
        if (result.IsFailure) return;
        
        foreach (Room affectedRoom in result.Value)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, affectedRoom.Id.ToString());
            
            await Clients
                .Group(affectedRoom.Id.ToString())
                .SendAsync(EventNames.PlayerListUpdated, affectedRoom.Players);
        }
    }
}