namespace DotRMediator;

/// <summary>
/// Publishes notifications to all registered handlers.
/// </summary>
/// <remarks>
/// Unlike requests, notifications may have zero or many handlers.
/// Handlers run sequentially in registration order.
/// </remarks>
public interface IPublisher
{
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
    Task Publish<TNotification>(
        TNotification notification,
        CancellationToken cancellationToken = default
    )
        where TNotification : INotification;

    /// <summary>
    /// Publishes a notification dynamically.
    /// </summary>
    /// <remarks>
    /// The object must implement <see cref="INotification"/>; otherwise an <see cref="ArgumentException"/> is thrown.
    /// </remarks>
    /// <param name="notification">The notification instance to publish.</param>
    /// <param name="cancellationToken">Token used to cancel handler execution.</param>
    /// <returns>A task that completes when all handlers finish.</returns>
    Task Publish(object notification, CancellationToken cancellationToken = default);
}
