using Shared.Application.Abstractions.Messaging;

namespace Application.CQRS.Room.Queries;

public sealed record GetRoomByIdQuery(Guid Id) : IQuery<Domain.Models.Room>;