using EventFlow.Modules.Events.Domain.Abstractions;
using MediatR;

namespace EventFlow.Modules.Events.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
