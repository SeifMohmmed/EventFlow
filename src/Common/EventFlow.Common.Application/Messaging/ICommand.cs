using EventFlow.Common.Domain;
using MediatR;

namespace EventFlow.Common.Application.Messaging;

/// <summary>
/// Represents a command that performs an action
/// without returning a value.
/// </summary>
public interface ICommand : IRequest<Result>, IBaseCommand;

/// <summary>
/// Represents a command that performs an action
/// and returns a value.
/// </summary>
/// <typeparam name="TResponse">The type of the returned value.</typeparam>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

/// <summary>
/// Marker interface used to identify all commands.
/// </summary>
public interface IBaseCommand;
