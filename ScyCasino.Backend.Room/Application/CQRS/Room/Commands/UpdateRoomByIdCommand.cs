using Shared.Application.Abstractions.Messaging;

namespace Application.CQRS.Room.Commands;

public record UpdateRoomByIdCommand(Guid Id, Domain.Models.Room Room) : ICommand;