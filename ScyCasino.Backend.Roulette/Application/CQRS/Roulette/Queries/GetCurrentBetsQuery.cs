using Shared.Application.Abstractions.Messaging;
using Shared.Application.Events.Roulette;

namespace Application.CQRS.Roulette.Queries;

public sealed record GetCurrentBetsQuery(Guid RoomId) : IQuery<List<PlacedRouletteBet>>;