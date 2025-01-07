using Shared.Application.Abstractions.Messaging;
using Shared.Kernel.Core;

namespace Application.CQRS.Room.Queries;

public sealed record GetAllRoomsQuery(GetAllContext Context) : IQuery<PaginatedResult<Domain.Models.Room>>;