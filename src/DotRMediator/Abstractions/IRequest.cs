namespace DotRMediator;

/// <summary>
/// Marks a type as a request that returns a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <remarks>
/// Implement this interface on command or query types dispatched through <see cref="ISender"/>.
/// Pair each request with an <see cref="IRequestHandler{TRequest, TResponse}"/> implementation.
/// </remarks>
/// <typeparam name="TResponse">The response type returned by the handler.</typeparam>
public interface IRequest<out TResponse> { }

/// <summary>
/// Marks a type as a request with no return value (equivalent to <see cref="IRequest{Unit}"/>).
/// </summary>
/// <remarks>
/// Use for fire-and-forget commands where the handler returns <see cref="Unit"/>.
/// Handlers may implement <see cref="IRequestHandler{TRequest}"/> instead of the two-type-parameter variant.
/// </remarks>
public interface IRequest : IRequest<Unit> { }
