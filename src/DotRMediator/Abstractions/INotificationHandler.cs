namespace DotRMediator;

/// <summary>
/// Handler for a specific notification.
/// </summary>
/// <remarks>
/// Register multiple handlers for the same notification type; all are invoked during publish.
/// Handlers run sequentially, not in parallel.
/// </remarks>
/// <typeparam name="TNotification">The notification type handled by this implementation.</typeparam>
public interface INotificationHandler<in TNotification>
    where TNotification : INotification
{
    /// <summary>
    /// Processes the notification.
    /// </summary>
    /// <remarks>
    /// Exceptions propagate to the caller and stop subsequent handlers from running.
    /// </remarks>
    /// <param name="notification">The notification instance to handle.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when handling finishes.</returns>
    Task Handle(TNotification notification, CancellationToken cancellationToken);
}
