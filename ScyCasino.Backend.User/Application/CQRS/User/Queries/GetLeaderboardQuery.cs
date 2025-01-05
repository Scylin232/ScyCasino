using Application.Abstractions.Messaging;
using SharedKernel.Core;

namespace Application.CQRS.User.Queries;

public sealed record GetLeaderboardQuery(GetAllContext Context) : IQuery<PaginatedResult<Domain.Models.User>>;