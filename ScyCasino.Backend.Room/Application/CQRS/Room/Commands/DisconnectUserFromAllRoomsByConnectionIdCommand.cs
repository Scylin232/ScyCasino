using System.Collections;
using Shared.Application.Abstractions.Messaging;
using Shared.Application.Abstractions.Messaging;

namespace Application.CQRS.Room.Commands;

public sealed record DisconnectUserFromAllRoomsByConnectionIdCommand(string ConnectionId) : ICommand<IEnumerable<Domain.Models.Room>>;