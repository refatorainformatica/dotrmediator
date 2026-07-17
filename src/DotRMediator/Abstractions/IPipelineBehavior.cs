namespace DotRMediator;

/// <summary>
/// Delegate representing the next step in a request pipeline.
/// </summary>
/// <remarks>
/// Each behavior wraps the delegate to form an onion-style chain ending at the request handler.
/// </remarks>
/// <typeparam name="TResponse">The request response type.</typeparam>
/// <returns>A task that completes with the response from the remaining pipeline.</returns>
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

/// <summary>
/// Pipeline behavior executed around a request handler.
/// </summary>
/// <remarks>
/// Behaviors are registered in DI and composed in reverse registration order.
/// Built-in behaviors handle pre/post processing and exception handling.
/// </remarks>
/// <typeparam name="TRequest">The request type flowing through the pipeline.</typeparam>
/// <typeparam name="TResponse">The response type produced by the pipeline.</typeparam>
public interface IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
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
    Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    );
}
