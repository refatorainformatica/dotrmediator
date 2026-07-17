namespace DotRMediator;

/// <summary>
/// Marks a type as a request that returns a stream of responses.
/// </summary>
/// <remarks>
/// Dispatched through <see cref="IMediator.CreateStream{TResponse}"/>.
/// Pair each stream request with an <see cref="IStreamRequestHandler{TRequest, TResponse}"/> implementation.
/// </remarks>
/// <typeparam name="TResponse">The type of each item emitted by the stream.</typeparam>
public interface IStreamRequest<out TResponse> { }
