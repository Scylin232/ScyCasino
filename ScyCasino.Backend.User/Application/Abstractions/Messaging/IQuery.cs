﻿using MediatR;
using SharedKernel.Core;

namespace Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;