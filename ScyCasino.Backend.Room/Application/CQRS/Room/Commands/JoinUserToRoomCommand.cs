using Application.Abstractions.Messaging;

namespace Application.CQRS.Room.Commands;

public sealed record JoinUserToRoomCommand(Guid RoomId, string UserToken) : ICommand<Domain.Models.Room>;