using Application.Abstractions.Messaging;
using SharedKernel.Core;

namespace Application.CQRS.Room.Queries;

public sealed record GetAllRoomsQuery(GetAllContext Context) : IQuery<PaginatedResult<Domain.Models.Room>>;