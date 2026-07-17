namespace DotRMediator;

/// <summary>
/// Handler for exceptions thrown during request processing.
/// </summary>
/// <remarks>
/// Invoked by <see cref="Behaviors.RequestExceptionProcessorBehavior{TRequest, TResponse}"/> when the pipeline throws.
/// Set <see cref="RequestExceptionHandlerState{TResponse}.Handled"/> and <see cref="RequestExceptionHandlerState{TResponse}.Response"/> to recover without rethrowing.
/// </remarks>
/// <typeparam name="TRequest">The request type associated with the exception.</typeparam>
/// <typeparam name="TResponse">The response type expected by the pipeline.</typeparam>
/// <typeparam name="TException">The exception type handled by this implementation.</typeparam>
public interface IRequestExceptionHandler<in TRequest, TResponse, in TException>
    where TRequest : notnull
    where TException : Exception
{
    /// <summary>
    /// Handles the exception and optionally sets a fallback response.
    /// </summary>
    /// <remarks>
    /// Handlers run in registration order until one sets <see cref="RequestExceptionHandlerState{TResponse}.Handled"/> to <c>true</c>.
    /// <see cref="OperationCanceledException"/> is not routed to exception handlers.
    /// </remarks>
    /// <param name="request">The request instance being processed when the exception occurred.</param>
    /// <param name="exception">The caught exception instance.</param>
    /// <param name="state">Mutable state shared across exception handlers for the current failure.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when handling finishes.</returns>
    Task Handle(
        TRequest request,
        TException exception,
        RequestExceptionHandlerState<TResponse> state,
        CancellationToken cancellationToken
    );
}
