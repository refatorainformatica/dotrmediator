using Microsoft.Extensions.DependencyInjection;

namespace DotRMediator.Internal;

/// <summary>
/// Base type for dynamically created request handler wrappers.
/// </summary>
/// <remarks>
/// Allows caching closed generic <see cref="RequestHandlerWrapper{TRequest, TResponse}"/> instances without exposing generic types.
/// </remarks>
internal abstract class RequestHandlerBase
{
    /// <summary>
    /// Dispatches a request through the pipeline for the wrapper's closed generic types.
    /// </summary>
    /// <remarks>
    /// The request is cast to the expected type inside the concrete wrapper.
    /// </remarks>
    /// <param name="request">The request instance to dispatch.</param>
    /// <param name="serviceProvider">The service provider used to resolve the handler and behaviors.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes with the handler response boxed as <see cref="object"/>.</returns>
    public abstract Task<object?> Handle(
        object request,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken
    );
}

/// <summary>
/// Typed wrapper that composes pipeline behaviors around a request handler.
/// </summary>
/// <remarks>
/// Created at runtime via reflection and cached by <see cref="RequestHandlerWrapperCache"/>.
/// Behaviors are reversed so the first registered behavior becomes the outermost layer.
/// </remarks>
/// <typeparam name="TRequest">The closed request type.</typeparam>
/// <typeparam name="TResponse">The closed response type.</typeparam>
internal sealed class RequestHandlerWrapper<TRequest, TResponse> : RequestHandlerBase
    where TRequest : notnull, IRequest<TResponse>
{
    /// <summary>
    /// Dispatches a request through the composed behavior chain and handler.
    /// </summary>
    /// <remarks>
    /// Resolves <see cref="IRequestHandler{TRequest, TResponse}"/> and all <see cref="IPipelineBehavior{TRequest, TResponse}"/> instances from DI.
    /// </remarks>
    /// <param name="request">The request instance to dispatch.</param>
    /// <param name="serviceProvider">The service provider used to resolve the handler and behaviors.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes with the handler response boxed as <see cref="object"/>.</returns>
    public override async Task<object?> Handle(
        object request,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken
    )
    {
        var typedRequest = (TRequest)request;
        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        RequestHandlerDelegate<TResponse> handlerDelegate = () =>
            handler.Handle(typedRequest, cancellationToken);

        var behaviors = serviceProvider
            .GetServices<IPipelineBehavior<TRequest, TResponse>>()
            .Reverse()
            .ToList();

        foreach (var behavior in behaviors)
        {
            var next = handlerDelegate;
            handlerDelegate = () => behavior.Handle(typedRequest, next, cancellationToken);
        }

        var response = await handlerDelegate().ConfigureAwait(false);
        return response;
    }
}

/// <summary>
/// Base type for dynamically created stream handler wrappers.
/// </summary>
/// <remarks>
/// Allows caching closed generic <see cref="StreamHandlerWrapper{TRequest, TResponse}"/> instances without exposing generic types.
/// </remarks>
internal abstract class StreamHandlerBase
{
    /// <summary>
    /// Dispatches a stream request through the pipeline for the wrapper's closed generic types.
    /// </summary>
    /// <remarks>
    /// The request is cast to the expected type inside the concrete wrapper.
    /// </remarks>
    /// <param name="request">The stream request instance to dispatch.</param>
    /// <param name="serviceProvider">The service provider used to resolve the handler and behaviors.</param>
    /// <param name="cancellationToken">Token used to cancel stream enumeration.</param>
    /// <returns>An async sequence of handler responses boxed as <see cref="object"/>.</returns>
    public abstract IAsyncEnumerable<object?> Handle(
        object request,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken
    );
}

/// <summary>
/// Typed wrapper that composes stream pipeline behaviors around a stream handler.
/// </summary>
/// <remarks>
/// Created at runtime via reflection and cached by <see cref="RequestHandlerWrapperCache"/>.
/// Behaviors are reversed so the first registered behavior becomes the outermost layer.
/// </remarks>
/// <typeparam name="TRequest">The closed stream request type.</typeparam>
/// <typeparam name="TResponse">The closed stream item type.</typeparam>
internal sealed class StreamHandlerWrapper<TRequest, TResponse> : StreamHandlerBase
    where TRequest : notnull, IStreamRequest<TResponse>
{
    /// <summary>
    /// Dispatches a stream request through the composed behavior chain and handler.
    /// </summary>
    /// <remarks>
    /// Resolves <see cref="IStreamRequestHandler{TRequest, TResponse}"/> and all <see cref="IStreamPipelineBehavior{TRequest, TResponse}"/> instances from DI.
    /// </remarks>
    /// <param name="request">The stream request instance to dispatch.</param>
    /// <param name="serviceProvider">The service provider used to resolve the handler and behaviors.</param>
    /// <param name="cancellationToken">Token used to cancel stream enumeration.</param>
    /// <returns>An async sequence of handler responses boxed as <see cref="object"/>.</returns>
    public override IAsyncEnumerable<object?> Handle(
        object request,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken
    )
    {
        var typedRequest = (TRequest)request;
        var handler = serviceProvider.GetRequiredService<
            IStreamRequestHandler<TRequest, TResponse>
        >();

        StreamHandlerDelegate<TResponse> handlerDelegate = () =>
            handler.Handle(typedRequest, cancellationToken);

        var behaviors = serviceProvider
            .GetServices<IStreamPipelineBehavior<TRequest, TResponse>>()
            .Reverse()
            .ToList();

        foreach (var behavior in behaviors)
        {
            var next = handlerDelegate;
            handlerDelegate = () => behavior.Handle(typedRequest, next, cancellationToken);
        }

        return StreamToObject(handlerDelegate());
    }

    /// <summary>
    /// Converts a typed async stream into a stream of boxed objects.
    /// </summary>
    /// <remarks>
    /// Used by the non-generic dispatch path in <see cref="Mediator.CreateStream{TResponse}"/>.
    /// </remarks>
    /// <param name="source">The typed stream produced by the inner pipeline.</param>
    /// <returns>An async sequence with each item boxed as <see cref="object"/>.</returns>
    private static async IAsyncEnumerable<object?> StreamToObject(
        IAsyncEnumerable<TResponse> source
    )
    {
        await foreach (var item in source.ConfigureAwait(false))
        {
            yield return item;
        }
    }
}
