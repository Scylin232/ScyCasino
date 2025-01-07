using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.CQRS.Roulette.Commands;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}