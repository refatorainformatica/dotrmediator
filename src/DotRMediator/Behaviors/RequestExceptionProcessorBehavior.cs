using Microsoft.Extensions.DependencyInjection;

namespace DotRMediator.Behaviors;

/// <summary>
/// Pipeline behavior that executes exception handlers registered via <see cref="IRequestExceptionHandler{TRequest, TResponse, TException}"/>.
/// </summary>
/// <remarks>
/// Registered automatically by <c>AddDotRMediator</c> as an open generic pipeline behavior.
/// When a handler marks the exception as handled, the fallback response is returned instead of rethrowing.
/// </remarks>
/// <typeparam name="TRequest">The request type associated with pipeline exceptions.</typeparam>
/// <typeparam name="TResponse">The response type expected by the pipeline.</typeparam>
public sealed class RequestExceptionProcessorBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Service provider used to resolve exception handlers at runtime.
    /// </summary>
    /// <remarks>
    /// Handlers are resolved by concrete exception type via reflection.
    /// </remarks>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of <see cref="RequestExceptionProcessorBehavior{TRequest, TResponse}"/>.
    /// </summary>
    /// <remarks>
    /// The service provider must contain registered <see cref="IRequestExceptionHandler{TRequest, TResponse, TException}"/> implementations.
    /// </remarks>
    /// <param name="serviceProvider">The service provider used to resolve exception handlers.</param>
    public RequestExceptionProcessorBehavior(IServiceProvider serviceProvider)
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
            var state = new RequestExceptionHandlerState<TResponse>();
            await ExecuteHandlers(request, exception, state, cancellationToken)
                .ConfigureAwait(false);

            if (state.Handled)
            {
                return state.Response!;
            }

            throw;
        }
    }

    /// <summary>
    /// Invokes all exception handlers matching the runtime exception type.
    /// </summary>
    /// <remarks>
    /// Stops when a handler sets <see cref="RequestExceptionHandlerState{TResponse}.Handled"/> to <c>true</c>.
    /// </remarks>
    /// <param name="request">The request instance being processed when the exception occurred.</param>
    /// <param name="exception">The caught exception instance.</param>
    /// <param name="state">Mutable state shared across exception handlers for the current failure.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when handler execution finishes.</returns>
    private async Task ExecuteHandlers(
        TRequest request,
        Exception exception,
        RequestExceptionHandlerState<TResponse> state,
        CancellationToken cancellationToken
    )
    {
        var exceptionType = exception.GetType();
        var handlerType = typeof(IRequestExceptionHandler<,,>).MakeGenericType(
            typeof(TRequest),
            typeof(TResponse),
            exceptionType
        );
        var handlers = _serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            var method = handlerType.GetMethod(
                nameof(IRequestExceptionHandler<TRequest, TResponse, Exception>.Handle)
            )!;
            var task = (Task)
                method.Invoke(handler, [request, exception, state, cancellationToken])!;
            await task.ConfigureAwait(false);

            if (state.Handled)
            {
                return;
            }
        }
    }
}
