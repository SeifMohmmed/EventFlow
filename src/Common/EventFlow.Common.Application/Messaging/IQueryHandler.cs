using EventFlow.Common.Domain;
using MediatR;

namespace EventFlow.Common.Application.Messaging;

/// <summary>
/// Defines a handler for queries.
/// </summary>
/// <typeparam name="TQuery">The query type.</typeparam>
/// <typeparam name="TResponse">The type of the returned value.</typeparam>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
