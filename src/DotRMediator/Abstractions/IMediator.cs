namespace DotRMediator;

/// <summary>
/// Facade that combines request dispatch and notification publishing.
/// </summary>
/// <remarks>
/// Use when a component needs both <see cref="ISender"/> and <see cref="IPublisher"/> capabilities.
/// Stream requests are dispatched through <see cref="CreateStream{TResponse}"/>.
/// </remarks>
public interface IMediator : ISender, IPublisher
{
    /// <summary>
    /// Creates a stream of responses for a request of type <see cref="IStreamRequest{TResponse}"/>.
    /// </summary>
    /// <remarks>
    /// Resolves the matching <see cref="IStreamRequestHandler{TRequest, TResponse}"/> and applies registered stream pipeline behaviors.
    /// Items are yielded as they are produced by the handler.
    /// </remarks>
    /// <typeparam name="TResponse">The type of each item emitted by the stream.</typeparam>
    /// <param name="request">The stream request instance.</param>
    /// <param name="cancellationToken">Token used to cancel enumeration.</param>
    /// <returns>An async sequence of handler responses.</returns>
    IAsyncEnumerable<TResponse> CreateStream<TResponse>(
        IStreamRequest<TResponse> request,
        CancellationToken cancellationToken = default);
}
