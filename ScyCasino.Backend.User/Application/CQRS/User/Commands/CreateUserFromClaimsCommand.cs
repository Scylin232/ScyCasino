using System.Security.Claims;
using Shared.Application.Abstractions.Messaging;

namespace Application.CQRS.User.Commands;

public sealed record CreateUserFromClaimsCommand(IEnumerable<Claim> Claims) : ICommand;