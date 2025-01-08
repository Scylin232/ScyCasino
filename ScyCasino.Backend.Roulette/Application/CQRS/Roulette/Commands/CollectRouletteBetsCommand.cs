using Shared.Application.Abstractions.Messaging;

namespace Application.CQRS.Roulette.Commands;

public sealed record CollectRouletteBetsCommand : ICommand;