namespace DotRMediator;

/// <summary>
/// Post-processor executed after a request handler.
/// </summary>
/// <remarks>
/// Invoked by <see cref="Behaviors.RequestPostProcessorBehavior{TRequest, TResponse}"/> after the handler returns.
/// Register multiple post-processors; they execute in DI registration order.
/// </remarks>
/// <typeparam name="TRequest">The request type processed after handling.</typeparam>
/// <typeparam name="TResponse">The response type produced by the handler.</typeparam>
public interface IRequestPostProcessor<in TRequest, in TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Executes post-processing logic.
    /// </summary>
    /// <remarks>
    /// Use for auditing, caching, or side effects that depend on the handler response.
    /// Runs only when the handler completes without throwing.
    /// </remarks>
    /// <param name="request">The request instance that was handled.</param>
    /// <param name="response">The response returned by the handler.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when post-processing finishes.</returns>
    Task Process(TRequest request, TResponse response, CancellationToken cancellationToken);
}
