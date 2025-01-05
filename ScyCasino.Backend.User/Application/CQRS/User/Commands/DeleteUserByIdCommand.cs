using Application.Abstractions.Messaging;

namespace Application.CQRS.User.Commands;

public sealed record DeleteUserByIdCommand(Guid Id) : ICommand;