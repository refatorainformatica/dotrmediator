namespace DotRMediator;

/// <summary>
/// Sends requests to their registered handlers.
/// </summary>
/// <remarks>
/// Register handlers with <c>AddDotRMediator</c> and resolve <see cref="ISender"/> from the DI container.
/// Typed and untyped overloads share the same pipeline and behavior chain.
/// </remarks>
public interface ISender
{
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
    Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default
    );

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
    Task<object?> Send(object request, CancellationToken cancellationToken = default);
}
