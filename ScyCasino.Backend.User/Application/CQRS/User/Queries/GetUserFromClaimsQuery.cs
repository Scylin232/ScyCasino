using System.Security.Claims;
using Application.Abstractions.Messaging;

namespace Application.CQRS.User.Queries;

public sealed record GetUserFromClaimsQuery(IEnumerable<Claim> Claims) : IQuery<Domain.Models.User>;