namespace DotRMediator;

/// <summary>
/// Marks a type as a notification/event that can be handled by multiple handlers.
/// </summary>
/// <remarks>
/// Notifications represent side effects or domain events and are published through <see cref="IPublisher"/>.
/// Multiple <see cref="INotificationHandler{TNotification}"/> implementations may exist for the same type.
/// </remarks>
public interface INotification { }
