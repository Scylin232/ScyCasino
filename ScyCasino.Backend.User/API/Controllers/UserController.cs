using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.User.Queries;
using Application.CQRS.User.Commands;
using Domain.Models;
using SharedKernel.Core;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(ISender sender) : ControllerBase
{
    [HttpGet("self")]
    [Authorize]
    public async Task<IActionResult> GetSelf()
    {
        GetUserFromClaimsQuery query = new(User.Claims);
        
        Result<User> result = await sender.Send(query);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    [HttpPost("oauth-complete")]
    [Authorize]
    public async Task<IActionResult> CreateUserFromClaims()
    {
        CreateUserFromClaimsCommand command = new(User.Claims);
        
        Result result = await sender.Send(command);
        
        return result.IsSuccess ? Ok() : BadRequest();
    }
    
    [HttpGet]
    [Authorize(Policy = "IsAdministrator")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        GetUserByIdQuery query = new(id);
        
        Result<User> result = await sender.Send(query);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    [HttpPatch]
    [Authorize(Policy = "IsAdministrator")]
    public async Task<IActionResult> UpdateUserById([FromQuery] Guid id, [FromBody] User user)
    {
        UpdateUserByIdCommand command = new(id, user);
        
        Result result = await sender.Send(command);
        
        return result.IsSuccess ? Ok() : BadRequest();
    }
    
    [HttpDelete]
    [Authorize(Policy = "IsAdministrator")]
    public async Task<IActionResult> DeleteUserById(Guid id)
    {
        DeleteUserByIdCommand command = new(id);
        
        Result result = await sender.Send(command);
        
        return result.IsSuccess ? Ok() : BadRequest();
    }
}