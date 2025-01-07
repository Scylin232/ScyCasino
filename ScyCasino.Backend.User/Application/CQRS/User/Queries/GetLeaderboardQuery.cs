using Shared.Application.Abstractions.Messaging;
using Shared.Kernel.Core;

namespace Application.CQRS.User.Queries;

public sealed record GetLeaderboardQuery(GetAllContext Context) : IQuery<PaginatedResult<Domain.Models.User>>;