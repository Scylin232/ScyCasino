using Shared.Application.Abstractions.Messaging;

namespace Application.CQRS.Room.Commands;

public sealed record JoinUserToRoomCommand(Guid RoomId, string UserToken, string ConnectionId) : ICommand<Domain.Models.Room>;