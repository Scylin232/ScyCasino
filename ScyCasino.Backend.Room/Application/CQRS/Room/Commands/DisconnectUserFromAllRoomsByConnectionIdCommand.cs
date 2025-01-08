using Shared.Application.Abstractions.Messaging;

namespace Application.CQRS.Room.Commands;

public sealed record DisconnectUserFromAllRoomsByConnectionIdCommand(string ConnectionId) : ICommand<IEnumerable<Domain.Models.Room>>;