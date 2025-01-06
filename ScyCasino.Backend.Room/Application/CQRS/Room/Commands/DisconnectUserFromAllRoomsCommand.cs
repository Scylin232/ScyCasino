using Application.Abstractions.Messaging;

namespace Application.CQRS.Room.Commands;

public sealed record DisconnectUserFromAllRoomsCommand(string UserToken) : ICommand<IEnumerable<Domain.Models.Room>>;