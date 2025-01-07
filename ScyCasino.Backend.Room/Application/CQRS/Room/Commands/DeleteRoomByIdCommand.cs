using Shared.Application.Abstractions.Messaging;

namespace Application.CQRS.Room.Commands;

public sealed record DeleteRoomByIdCommand(Guid Id) : ICommand;