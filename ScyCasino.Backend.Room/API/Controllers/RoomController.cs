using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.CQRS.Room.Commands;
using Application.CQRS.Room.Queries;
using Domain.Models;
using SharedKernel.Core;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController(ISender sender) : ControllerBase
{
    [HttpGet("all")]
    public async Task<IActionResult> GetAllRooms(int page, int count)
    {
        GetAllRoomsQuery query = new(new GetAllContext
        {
            Page = page,
            Count = count
        });
        
        Result<PaginatedResult<Room>> result = await sender.Send(query);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRoomById(Guid id)
    {
        GetRoomByIdQuery query = new(id);
        
        Result<Room> result = await sender.Send(query);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateRoom(Room room)
    {
        CreateRoomCommand command = new(room);
        
        Result result = await sender.Send(command);
        
        return result.IsSuccess ? Ok() : BadRequest();
    }
    
    [HttpPatch]
    [Authorize(Policy = "IsAdministrator")]
    public async Task<IActionResult> UpdateRoom(Guid id, Room room)
    {
        UpdateRoomByIdCommand command = new(id, room);
        
        Result result = await sender.Send(command);
        
        return result.IsSuccess ? Ok() : BadRequest();
    }
    
    [HttpDelete]
    [Authorize(Policy = "IsAdministrator")]
    public async Task<IActionResult> DeleteRoom(Guid id)
    {
        DeleteRoomByIdCommand command = new(id);
        
        Result result = await sender.Send(command);
        
        return result.IsSuccess ? Ok() : BadRequest();
    }
}