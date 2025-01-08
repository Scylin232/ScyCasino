using System.Security.Claims;
using Application.CQRS.Roulette.Commands;
using Application.CQRS.Roulette.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Events.Roulette;
using Shared.Kernel.Core;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RouletteController(ISender sender) : ControllerBase
{
    [HttpPost("bet")]
    [Authorize]
    public async Task<IActionResult> PlaceRouletteBet(Guid roomId, int amount, RouletteBetType betType, int[] betValues)
    {
        Claim? subjectClaim = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        
        if (subjectClaim is null)
            return BadRequest();
        
        string issuer = subjectClaim.Issuer;
        string subject = subjectClaim.Value;
        
        PlaceRouletteBetCommand command = new(roomId, issuer, subject, amount, betType, betValues);
        
        Result result = await sender.Send(command);
        
        return result.IsSuccess ? Ok() : BadRequest();
    }
    
    [HttpGet("current-bets")]
    [Authorize]
    public async Task<IActionResult> GetCurrentBets(Guid roomId)
    {
        GetCurrentBetsQuery query = new(roomId);
        
        Result<List<PlacedRouletteBet>> result = await sender.Send(query);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    [HttpGet("all/bets")]
    [Authorize(Policy = "IsAdministrator")]
    public async Task<IActionResult> GetAllBets(int page, int count)
    {
        GetAllContext context = new()
        {
            Page = page,
            Count = count
        };
        
        GetAllBetsQuery query = new(context);
        
        Result<PaginatedResult<RouletteBet>> result = await sender.Send(query);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("all/states")]
    [Authorize(Policy = "IsAdministrator")]
    public async Task<IActionResult> GetAllGameStates(int page, int count)
    {
        GetAllContext context = new()
        {
            Page = page,
            Count = count
        };
        
        GetAllGameStatesQuery query = new(context);
        
        Result<PaginatedResult<RouletteGameState>> result = await sender.Send(query);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}