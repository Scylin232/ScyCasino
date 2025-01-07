using Application.CQRS.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Kernel.Core;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetLeaderboard(int page, int count)
    {
        GetAllContext leaderboardContext = new()
        {
            Page = page,
            Count = count,
            SortColumn = nameof(Domain.Models.User.Coins).ToLower(),
            IsAscending = false
        };
        
        GetLeaderboardQuery leaderboardQuery = new(leaderboardContext);
        
        Result<PaginatedResult<Domain.Models.User>> result = await sender.Send(leaderboardQuery);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}