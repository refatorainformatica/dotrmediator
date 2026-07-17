namespace DotRMediator;

/// <summary>
/// Delegate representing the next step in a stream request pipeline.
/// </summary>
/// <remarks>
/// Each stream behavior wraps the delegate to form a chain ending at the stream handler.
/// </remarks>
/// <typeparam name="TResponse">The type of each item emitted by the stream.</typeparam>
/// <returns>An async sequence produced by the remaining pipeline.</returns>
public delegate IAsyncEnumerable<TResponse> StreamHandlerDelegate<TResponse>();

/// <summary>
/// Pipeline behavior executed around a stream request handler.
/// </summary>
/// <remarks>
/// Behaviors are registered in DI and composed in reverse registration order.
/// Use for cross-cutting concerns such as logging or filtering stream items.
/// </remarks>
/// <typeparam name="TRequest">The stream request type flowing through the pipeline.</typeparam>
/// <typeparam name="TResponse">The type of each item emitted by the stream.</typeparam>
public interface IStreamPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Executes the behavior and invokes the next pipeline step.
    /// </summary>
    /// <remarks>
    /// Call <paramref name="next"/> to continue the pipeline; omit the call to short-circuit.
    /// Behaviors may transform or filter items from the inner stream.
    /// </remarks>
    /// <param name="request">The stream request instance being processed.</param>
    /// <param name="next">The delegate for the next pipeline stage.</param>
    /// <param name="cancellationToken">Token used to cancel stream enumeration.</param>
    /// <returns>An async sequence of pipeline response items.</returns>
    IAsyncEnumerable<TResponse> Handle(
        TRequest request,
        StreamHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    );
}
