using DotRMediator.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace DotRMediator;

/// <summary>
/// Default implementation of <see cref="IMediator"/>.
/// </summary>
/// <remarks>
/// Resolves handlers and pipeline behaviors from the provided <see cref="IServiceProvider"/>.
/// Registered as the default <see cref="IMediator"/>, <see cref="ISender"/>, and <see cref="IPublisher"/> implementation via <c>AddDotRMediator</c>.
/// </remarks>
public sealed class Mediator : IMediator
{
    /// <summary>
    /// Service provider used to resolve handlers, behaviors, and notification handlers.
    /// </summary>
    /// <remarks>
    /// Must outlive the mediator instance and contain all registered handler types.
    /// </remarks>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of <see cref="Mediator"/>.
    /// </summary>
    /// <remarks>
    /// Typically resolved from DI; the same provider is used for the lifetime of the mediator.
    /// </remarks>
    /// <param name="serviceProvider">The service provider used to resolve handlers and behaviors.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceProvider"/> is <c>null</c>.</exception>
    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider =
            serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <summary>
    /// Sends a typed request and returns the handler response.
    /// </summary>
    /// <remarks>
    /// The request type must implement <see cref="IRequest{TResponse}"/> and have a matching <see cref="IRequestHandler{TRequest, TResponse}"/> registered.
    /// Pipeline behaviors run before the handler executes.
    /// </remarks>
    /// <typeparam name="TResponse">The response type declared by the request.</typeparam>
    /// <param name="request">The request instance to dispatch.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes with the handler response.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is <c>null</c>.</exception>
    public Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        return SendCore<TResponse>(request, cancellationToken);
    }

    /// <summary>
    /// Sends a request dynamically, resolving the handler at runtime.
    /// </summary>
    /// <remarks>
    /// Uses reflection to determine the request and response types from the runtime object.
    /// Throws when the object does not implement <see cref="IRequest{TResponse}"/> or <see cref="IRequest"/>.
    /// </remarks>
    /// <param name="request">The request instance to dispatch.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes with the handler response boxed as <see cref="object"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is <c>null</c>.</exception>
    public Task<object?> Send(object request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var (requestType, responseType) = RequestTypeHelper.GetRequestTypes(request);
        var wrapper = RequestHandlerWrapperCache.GetOrAdd(requestType, responseType);
        return wrapper.Handle(request, _serviceProvider, cancellationToken);
    }

    /// <summary>
    /// Publishes a typed notification.
    /// </summary>
    /// <remarks>
    /// All registered <see cref="INotificationHandler{TNotification}"/> instances for the notification type are invoked.
    /// No exception is thrown when no handlers are registered.
    /// </remarks>
    /// <typeparam name="TNotification">The notification type.</typeparam>
    /// <param name="notification">The notification instance to publish.</param>
    /// <param name="cancellationToken">Token used to cancel handler execution.</param>
    /// <returns>A task that completes when all handlers finish.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="notification"/> is <c>null</c>.</exception>
    public Task Publish<TNotification>(
        TNotification notification,
        CancellationToken cancellationToken = default
    )
        where TNotification : INotification
    {
        ArgumentNullException.ThrowIfNull(notification);
        return PublishCore(notification, cancellationToken);
    }

    /// <summary>
    /// Publishes a notification dynamically.
    /// </summary>
    /// <remarks>
    /// The object must implement <see cref="INotification"/>; otherwise an <see cref="ArgumentException"/> is thrown.
    /// </remarks>
    /// <param name="notification">The notification instance to publish.</param>
    /// <param name="cancellationToken">Token used to cancel handler execution.</param>
    /// <returns>A task that completes when all handlers finish.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="notification"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="notification"/> does not implement <see cref="INotification"/>.</exception>
    public Task Publish(object notification, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(notification);

        if (notification is not INotification)
        {
            throw new ArgumentException(
                "The provided object does not implement INotification.",
                nameof(notification)
            );
        }

        return PublishCore(notification, cancellationToken);
    }

    /// <summary>
    /// Creates a stream of responses for a request of type <see cref="IStreamRequest{TResponse}"/>.
    /// </summary>
    /// <remarks>
    /// Resolves the matching <see cref="IStreamRequestHandler{TRequest, TResponse}"/> and applies registered stream pipeline behaviors.
    /// Items are yielded as they are produced by the handler.
    /// </remarks>
    /// <typeparam name="TResponse">The type of each item emitted by the stream.</typeparam>
    /// <param name="request">The stream request instance.</param>
    /// <param name="cancellationToken">Token used to cancel enumeration.</param>
    /// <returns>An async sequence of handler responses.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is <c>null</c>.</exception>
    public async IAsyncEnumerable<TResponse> CreateStream<TResponse>(
        IStreamRequest<TResponse> request,
        [System.Runtime.CompilerServices.EnumeratorCancellation]
            CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestType = request.GetType();
        var wrapper = RequestHandlerWrapperCache.GetStreamOrAdd(requestType, typeof(TResponse));

        await foreach (
            var item in wrapper
                .Handle(request, _serviceProvider, cancellationToken)
                .ConfigureAwait(false)
        )
        {
            yield return (TResponse)item!;
        }
    }

    /// <summary>
    /// Sends a request using a cached handler wrapper for the runtime request type.
    /// </summary>
    /// <remarks>
    /// Shared by the typed <see cref="Send{TResponse}"/> overload after null validation.
    /// </remarks>
    /// <typeparam name="TResponse">The expected response type.</typeparam>
    /// <param name="request">The request instance to dispatch.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes with the handler response.</returns>
    private async Task<TResponse> SendCore<TResponse>(
        object request,
        CancellationToken cancellationToken
    )
    {
        var requestType = request.GetType();
        var wrapper = RequestHandlerWrapperCache.GetOrAdd(requestType, typeof(TResponse));
        var result = await wrapper
            .Handle(request, _serviceProvider, cancellationToken)
            .ConfigureAwait(false);
        return (TResponse)result!;
    }

    /// <summary>
    /// Invokes all notification handlers registered for the runtime notification type.
    /// </summary>
    /// <remarks>
    /// Handlers are resolved from the service provider and invoked sequentially.
    /// </remarks>
    /// <param name="notification">The notification instance to publish.</param>
    /// <param name="cancellationToken">Token used to cancel handler execution.</param>
    /// <returns>A task that completes when all handlers finish.</returns>
    private async Task PublishCore(object notification, CancellationToken cancellationToken)
    {
        var notificationType = notification.GetType();
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notificationType);
        var handlers = _serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            var handleMethod = handlerType.GetMethod(
                nameof(INotificationHandler<INotification>.Handle)
            )!;
            var task = (Task)handleMethod.Invoke(handler, [notification, cancellationToken])!;
            await task.ConfigureAwait(false);
        }
    }
}
