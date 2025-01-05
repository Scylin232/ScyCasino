using Application.Abstractions.Messaging;

namespace Application.CQRS.User.Queries;

public sealed record GetUserByIdQuery(Guid Id) : IQuery<Domain.Models.User>;