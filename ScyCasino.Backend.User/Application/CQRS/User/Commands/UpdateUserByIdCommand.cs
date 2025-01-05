using Application.Abstractions.Messaging;

namespace Application.CQRS.User.Commands;

public sealed record UpdateUserByIdCommand(Guid Id, Domain.Models.User User) : ICommand;