namespace DotRMediator.Behaviors;

/// <summary>
/// Pipeline behavior that executes all registered <see cref="IRequestPostProcessor{TRequest, TResponse}"/> instances.
/// </summary>
/// <remarks>
/// Registered automatically by <c>AddDotRMediator</c> as an open generic pipeline behavior.
/// Post-processors run in DI registration order after the handler returns successfully.
/// </remarks>
/// <typeparam name="TRequest">The request type processed after handling.</typeparam>
/// <typeparam name="TResponse">The response type produced by the handler.</typeparam>
public sealed class RequestPostProcessorBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Post-processors resolved for the current request and response types.
    /// </summary>
    /// <remarks>
    /// May be empty when no <see cref="IRequestPostProcessor{TRequest, TResponse}"/> implementations are registered.
    /// </remarks>
    private readonly IEnumerable<IRequestPostProcessor<TRequest, TResponse>> _postProcessors;

    /// <summary>
    /// Initializes a new instance of <see cref="RequestPostProcessorBehavior{TRequest, TResponse}"/>.
    /// </summary>
    /// <remarks>
    /// Post-processors are injected by the DI container for each closed generic request type.
    /// </remarks>
    /// <param name="postProcessors">The post-processors to invoke after the handler.</param>
    public RequestPostProcessorBehavior(
        IEnumerable<IRequestPostProcessor<TRequest, TResponse>> postProcessors
    )
    {
        _postProcessors = postProcessors;
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
        var response = await next().ConfigureAwait(false);

        foreach (var processor in _postProcessors)
        {
            await processor.Process(request, response, cancellationToken).ConfigureAwait(false);
        }

        return response;
    }
}
