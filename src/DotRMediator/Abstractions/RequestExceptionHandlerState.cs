namespace DotRMediator;

/// <summary>
/// Shared state during exception handling for a request.
/// </summary>
/// <remarks>
/// Passed to each <see cref="IRequestExceptionHandler{TRequest, TResponse, TException}"/> in the chain.
/// The first handler that sets <see cref="Handled"/> stops further handlers and returns <see cref="Response"/>.
/// </remarks>
/// <typeparam name="TResponse">The response type expected by the pipeline.</typeparam>
public class RequestExceptionHandlerState<TResponse>
{
    /// <summary>
    /// Indicates whether the exception was handled and a response was set.
    /// </summary>
    /// <remarks>
    /// When <c>true</c>, the pipeline returns <see cref="Response"/> instead of rethrowing.
    /// </remarks>
    public bool Handled { get; set; }

    /// <summary>
    /// Response set by the exception handler, when applicable.
    /// </summary>
    /// <remarks>
    /// Read only when <see cref="Handled"/> is <c>true</c>; may be default for value types.
    /// </remarks>
    public TResponse? Response { get; set; }
}
