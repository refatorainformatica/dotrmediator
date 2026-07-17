namespace DotRMediator;

/// <summary>
/// Action executed when an exception occurs during request processing.
/// </summary>
/// <remarks>
/// Invoked by <see cref="Behaviors.RequestExceptionActionProcessorBehavior{TRequest, TResponse}"/> before the exception is rethrown.
/// Use for logging or metrics; does not suppress exceptions.
/// </remarks>
/// <typeparam name="TRequest">The request type associated with the exception.</typeparam>
/// <typeparam name="TException">The exception type handled by this implementation.</typeparam>
public interface IRequestExceptionAction<in TRequest, in TException>
    where TRequest : notnull
    where TException : Exception
{
    /// <summary>
    /// Executes the exception action (e.g. logging, metrics).
    /// </summary>
    /// <remarks>
    /// All matching actions run even when multiple are registered.
    /// The original exception is always rethrown after actions complete.
    /// </remarks>
    /// <param name="request">The request instance being processed when the exception occurred.</param>
    /// <param name="exception">The caught exception instance.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the action finishes.</returns>
    Task Execute(TRequest request, TException exception, CancellationToken cancellationToken);
}
