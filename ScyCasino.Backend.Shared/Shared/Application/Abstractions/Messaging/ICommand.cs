using MediatR;
using Shared.Kernel.Core;

namespace Shared.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>;
public interface ICommand<TResponse> : IRequest<Result<TResponse>>;