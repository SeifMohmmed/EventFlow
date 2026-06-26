using EventFlow.Common.Domain;
using MediatR;

namespace EventFlow.Common.Application.Messaging;

/// <summary>
/// Represents a query that retrieves data
/// without modifying application state.
/// </summary>
/// <typeparam name="TResponse">The type of the returned value.</typeparam>
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
