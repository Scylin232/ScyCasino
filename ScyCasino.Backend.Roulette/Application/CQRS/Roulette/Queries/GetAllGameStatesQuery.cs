using Domain.Models;
using Shared.Application.Abstractions.Messaging;
using Shared.Kernel.Core;

namespace Application.CQRS.Roulette.Queries;

public sealed record GetAllGameStatesQuery(GetAllContext Context) : IQuery<PaginatedResult<RouletteGameState>>;