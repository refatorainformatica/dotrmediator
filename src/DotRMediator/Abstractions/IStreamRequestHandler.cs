namespace DotRMediator;

/// <summary>
/// Handler for requests that return streams.
/// </summary>
/// <remarks>
/// Register implementations via assembly scanning in <c>AddDotRMediator</c>.
/// Stream pipeline behaviors wrap the handler similarly to request pipelines.
/// </remarks>
/// <typeparam name="TRequest">The stream request type handled by this implementation.</typeparam>
/// <typeparam name="TResponse">The type of each item emitted by the stream.</typeparam>
public interface IStreamRequestHandler<in TRequest, out TResponse>
    where TRequest : IStreamRequest<TResponse>
{
    /// <summary>
    /// Processes the request and returns a stream of responses.
    /// </summary>
    /// <remarks>
    /// Items are yielded lazily; enumeration drives execution.
    /// Honor <paramref name="cancellationToken"/> when producing items.
    /// </remarks>
    /// <param name="request">The stream request instance to handle.</param>
    /// <param name="cancellationToken">Token used to cancel stream production.</param>
    /// <returns>An async sequence of response items.</returns>
    IAsyncEnumerable<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
