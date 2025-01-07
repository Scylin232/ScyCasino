using Shared.Application.Abstractions.Messaging;
using Domain.Models;

namespace Application.CQRS.Roulette.Commands;

public sealed record PlaceRouletteBetCommand(Guid RoomId, string Issuer, string Subject, int Amount, RouletteBetType BetType, int[] BetValues) : ICommand;