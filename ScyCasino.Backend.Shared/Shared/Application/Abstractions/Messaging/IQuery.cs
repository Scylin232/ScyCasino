using MediatR;
using Shared.Kernel.Core;

namespace Shared.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;