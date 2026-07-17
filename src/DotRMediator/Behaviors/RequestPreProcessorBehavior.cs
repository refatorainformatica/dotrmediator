namespace DotRMediator.Behaviors;

/// <summary>
/// Pipeline behavior that executes all registered <see cref="IRequestPreProcessor{TRequest}"/> instances.
/// </summary>
/// <remarks>
/// Registered automatically by <c>AddDotRMediator</c> as an open generic pipeline behavior.
/// Pre-processors run in DI registration order before the inner pipeline delegate.
/// </remarks>
/// <typeparam name="TRequest">The request type processed before handling.</typeparam>
/// <typeparam name="TResponse">The response type produced by the pipeline.</typeparam>
public sealed class RequestPreProcessorBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Pre-processors resolved for the current request type.
    /// </summary>
    /// <remarks>
    /// May be empty when no <see cref="IRequestPreProcessor{TRequest}"/> implementations are registered.
    /// </remarks>
    private readonly IEnumerable<IRequestPreProcessor<TRequest>> _preProcessors;

    /// <summary>
    /// Initializes a new instance of <see cref="RequestPreProcessorBehavior{TRequest, TResponse}"/>.
    /// </summary>
    /// <remarks>
    /// Pre-processors are injected by the DI container for each closed generic request type.
    /// </remarks>
    /// <param name="preProcessors">The pre-processors to invoke before the handler.</param>
    public RequestPreProcessorBehavior(IEnumerable<IRequestPreProcessor<TRequest>> preProcessors)
    {
        _preProcessors = preProcessors;
    }

    /// <summary>
    /// Executes the behavior and invokes the next pipeline step.
    /// </summary>
    /// <remarks>
    /// Call <paramref name="next"/> to continue the pipeline; omit the call to short-circuit.
    /// Behaviors may run logic before, after, or around the next delegate.
    /// </remarks>
    /// <param name="request">The request instance being processed.</param>
    /// <param name="next">The delegate for the next pipeline stage.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes with the pipeline response.</returns>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        foreach (var processor in _preProcessors)
        {
            await processor.Process(request, cancellationToken).ConfigureAwait(false);
        }

        return await next().ConfigureAwait(false);
    }
}
