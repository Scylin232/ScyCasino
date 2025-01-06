using Application.Abstractions.Messaging;

namespace Application.CQRS.Room.Commands;

public sealed record CreateRoomCommand(Domain.Models.Room Room) : ICommand;