using System.Collections.Concurrent;
using System.Reflection;

namespace DotRMediator.Internal;

/// <summary>
/// Caches dynamically created handler wrappers to avoid repeated reflection cost.
/// </summary>
/// <remarks>
/// Thread-safe via <see cref="ConcurrentDictionary{TKey, TValue}"/>.
/// Separate caches exist for request and stream dispatch paths.
/// </remarks>
internal static class RequestHandlerWrapperCache
{
    /// <summary>
    /// Cache of request handler wrappers keyed by request and response types.
    /// </summary>
    /// <remarks>
    /// Populated lazily on first dispatch for each type pair.
    /// </remarks>
    private static readonly ConcurrentDictionary<
        (Type Request, Type Response),
        RequestHandlerBase
    > RequestCache = new();

    /// <summary>
    /// Cache of stream handler wrappers keyed by request and response types.
    /// </summary>
    /// <remarks>
    /// Populated lazily on first stream dispatch for each type pair.
    /// </remarks>
    private static readonly ConcurrentDictionary<
        (Type Request, Type Response),
        StreamHandlerBase
    > StreamCache = new();

    /// <summary>
    /// Gets or creates a cached request handler wrapper for the specified types.
    /// </summary>
    /// <remarks>
    /// Instantiates <see cref="RequestHandlerWrapper{TRequest, TResponse}"/> via <see cref="Activator.CreateInstance(Type)"/> on cache miss.
    /// </remarks>
    /// <param name="requestType">The runtime request type.</param>
    /// <param name="responseType">The runtime response type.</param>
    /// <returns>A wrapper that dispatches requests of the given types.</returns>
    public static RequestHandlerBase GetOrAdd(Type requestType, Type responseType)
    {
        return RequestCache.GetOrAdd(
            (requestType, responseType),
            static key =>
            {
                var wrapperType = typeof(RequestHandlerWrapper<,>).MakeGenericType(
                    key.Request,
                    key.Response
                );
                return (RequestHandlerBase)Activator.CreateInstance(wrapperType)!;
            }
        );
    }

    /// <summary>
    /// Gets or creates a cached stream handler wrapper for the specified types.
    /// </summary>
    /// <remarks>
    /// Instantiates <see cref="StreamHandlerWrapper{TRequest, TResponse}"/> via <see cref="Activator.CreateInstance(Type)"/> on cache miss.
    /// </remarks>
    /// <param name="requestType">The runtime stream request type.</param>
    /// <param name="responseType">The runtime stream item type.</param>
    /// <returns>A wrapper that dispatches stream requests of the given types.</returns>
    public static StreamHandlerBase GetStreamOrAdd(Type requestType, Type responseType)
    {
        return StreamCache.GetOrAdd(
            (requestType, responseType),
            static key =>
            {
                var wrapperType = typeof(StreamHandlerWrapper<,>).MakeGenericType(
                    key.Request,
                    key.Response
                );
                return (StreamHandlerBase)Activator.CreateInstance(wrapperType)!;
            }
        );
    }
}

/// <summary>
/// Resolves request and response types from runtime request objects.
/// </summary>
/// <remarks>
/// Used by the untyped <see cref="Mediator.Send(object, CancellationToken)"/> overload.
/// </remarks>
internal static class RequestTypeHelper
{
    /// <summary>
    /// Determines the request and response types implemented by a runtime request object.
    /// </summary>
    /// <remarks>
    /// Checks for <see cref="IRequest{TResponse}"/>, <see cref="IStreamRequest{TResponse}"/>, or non-generic <see cref="IRequest"/>.
    /// </remarks>
    /// <param name="request">The request instance to inspect.</param>
    /// <returns>A tuple of the concrete request type and resolved response type.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the object does not implement a supported request interface.</exception>
    public static (Type RequestType, Type ResponseType) GetRequestTypes(object request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestType = request.GetType();

        foreach (var interfaceType in requestType.GetInterfaces())
        {
            if (!interfaceType.IsGenericType)
            {
                continue;
            }

            var definition = interfaceType.GetGenericTypeDefinition();

            if (definition == typeof(IRequest<>))
            {
                return (requestType, interfaceType.GetGenericArguments()[0]);
            }

            if (definition == typeof(IStreamRequest<>))
            {
                return (requestType, interfaceType.GetGenericArguments()[0]);
            }
        }

        if (request is IRequest)
        {
            return (requestType, typeof(Unit));
        }

        throw new InvalidOperationException(
            $"Type '{requestType.FullName}' does not implement IRequest<TResponse> or IRequest."
        );
    }

    /// <summary>
    /// Determines whether an object implements <see cref="INotification"/>.
    /// </summary>
    /// <remarks>
    /// Used for runtime type checks before publishing notifications.
    /// </remarks>
    /// <param name="notification">The object to inspect.</param>
    /// <returns><c>true</c> when the object implements <see cref="INotification"/>; otherwise <c>false</c>.</returns>
    public static bool IsNotification(object notification)
    {
        return notification is INotification;
    }
}

/// <summary>
/// Extension methods for strict service resolution.
/// </summary>
/// <remarks>
/// Mirrors <c>GetRequiredService</c> from Microsoft.Extensions.DependencyInjection for non-generic resolution.
/// </remarks>
internal static class ServiceProviderExtensions
{
    /// <summary>
    /// Resolves a required service or throws when it is not registered.
    /// </summary>
    /// <remarks>
    /// Throws <see cref="InvalidOperationException"/> instead of returning <c>null</c>.
    /// </remarks>
    /// <param name="serviceProvider">The service provider to query.</param>
    /// <param name="serviceType">The service type to resolve.</param>
    /// <returns>The resolved service instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no service is registered for <paramref name="serviceType"/>.</exception>
    public static object GetRequiredService(this IServiceProvider serviceProvider, Type serviceType)
    {
        var service = serviceProvider.GetService(serviceType);

        if (service is null)
        {
            throw new InvalidOperationException(
                $"No service registered for type '{serviceType.FullName}'."
            );
        }

        return service;
    }
}
