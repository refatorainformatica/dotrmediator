namespace DotRMediator;

/// <summary>
/// Handler for a request that returns <typeparamref name="TResponse"/>.
/// </summary>
/// <remarks>
/// Register implementations via assembly scanning in <c>AddDotRMediator</c>.
/// Only one handler per request type is resolved through DI.
/// </remarks>
/// <typeparam name="TRequest">The request type handled by this implementation.</typeparam>
/// <typeparam name="TResponse">The response type produced by the handler.</typeparam>
public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Processes the request and returns the response.
    /// </summary>
    /// <remarks>
    /// Pipeline behaviors, pre-processors, and post-processors run outside this method.
    /// Throw to propagate errors to exception handlers or the caller.
    /// </remarks>
    /// <param name="request">The request instance to handle.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes with the handler response.</returns>
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Handler for a request with no return value.
/// </summary>
/// <remarks>
/// Provides a convenience <see cref="Handle"/> overload that returns <see cref="Task"/> instead of <see cref="Task{Unit}"/>.
/// The explicit interface implementation bridges to <see cref="IRequestHandler{TRequest, TResponse}"/>.
/// </remarks>
/// <typeparam name="TRequest">The request type handled by this implementation.</typeparam>
public interface IRequestHandler<in TRequest> : IRequestHandler<TRequest, Unit>
    where TRequest : IRequest
{
    /// <summary>
    /// Processes the request.
    /// </summary>
    /// <remarks>
    /// Called by the mediator pipeline for requests implementing <see cref="IRequest"/> without a typed response.
    /// </remarks>
    /// <param name="request">The request instance to handle.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when handling finishes.</returns>
    new Task Handle(TRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Adapts the void handler to the <see cref="IRequestHandler{TRequest, TResponse}"/> contract.
    /// </summary>
    /// <remarks>
    /// Invokes <see cref="Handle"/> and returns <see cref="Unit.Value"/> on success.
    /// </remarks>
    /// <param name="request">The request instance to handle.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes with <see cref="Unit.Value"/>.</returns>
    async Task<Unit> IRequestHandler<TRequest, Unit>.Handle(
        TRequest request,
        CancellationToken cancellationToken
    )
    {
        await Handle(request, cancellationToken);
        return Unit.Value;
    }
}
