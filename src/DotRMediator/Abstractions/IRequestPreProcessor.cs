namespace DotRMediator;

/// <summary>
/// Pre-processor executed before a request handler.
/// </summary>
/// <remarks>
/// Invoked by <see cref="Behaviors.RequestPreProcessorBehavior{TRequest, TResponse}"/> before the handler runs.
/// Register multiple pre-processors; they execute in DI registration order.
/// </remarks>
/// <typeparam name="TRequest">The request type processed before handling.</typeparam>
public interface IRequestPreProcessor<in TRequest>
    where TRequest : notnull
{
    /// <summary>
    /// Executes pre-processing logic.
    /// </summary>
    /// <remarks>
    /// Use for validation, enrichment, or logging that must run before the handler.
    /// Throw to abort the pipeline before the handler executes.
    /// </remarks>
    /// <param name="request">The request instance being processed.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when pre-processing finishes.</returns>
    Task Process(TRequest request, CancellationToken cancellationToken);
}
