using Microsoft.Extensions.DependencyInjection;

namespace DotRMediator.Behaviors;

/// <summary>
/// Pipeline behavior that executes exception actions registered via <see cref="IRequestExceptionAction{TRequest, TException}"/>.
/// </summary>
/// <remarks>
/// Registered automatically by <c>AddDotRMediator</c> as an open generic pipeline behavior.
/// Actions run for side effects such as logging; the original exception is always rethrown.
/// </remarks>
/// <typeparam name="TRequest">The request type associated with pipeline exceptions.</typeparam>
/// <typeparam name="TResponse">The response type expected by the pipeline.</typeparam>
public sealed class RequestExceptionActionProcessorBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Service provider used to resolve exception actions at runtime.
    /// </summary>
    /// <remarks>
    /// Actions are resolved by concrete exception type via reflection.
    /// </remarks>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of <see cref="RequestExceptionActionProcessorBehavior{TRequest, TResponse}"/>.
    /// </summary>
    /// <remarks>
    /// The service provider must contain registered <see cref="IRequestExceptionAction{TRequest, TException}"/> implementations.
    /// </remarks>
    /// <param name="serviceProvider">The service provider used to resolve exception actions.</param>
    public RequestExceptionActionProcessorBehavior(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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
        try
        {
            return await next().ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            await ExecuteActions(request, exception, cancellationToken).ConfigureAwait(false);
            throw;
        }
    }

    /// <summary>
    /// Invokes all exception actions matching the runtime exception type.
    /// </summary>
    /// <remarks>
    /// Every matching action runs; none can suppress the exception.
    /// </remarks>
    /// <param name="request">The request instance being processed when the exception occurred.</param>
    /// <param name="exception">The caught exception instance.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when all actions finish.</returns>
    private async Task ExecuteActions(
        TRequest request,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var exceptionType = exception.GetType();
        var actionType = typeof(IRequestExceptionAction<,>).MakeGenericType(
            typeof(TRequest),
            exceptionType
        );
        var actions = _serviceProvider.GetServices(actionType);

        foreach (var action in actions)
        {
            var method = actionType.GetMethod(
                nameof(IRequestExceptionAction<TRequest, Exception>.Execute)
            )!;
            var task = (Task)method.Invoke(action, [request, exception, cancellationToken])!;
            await task.ConfigureAwait(false);
        }
    }
}
