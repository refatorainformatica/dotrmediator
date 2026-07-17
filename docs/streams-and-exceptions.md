# Streams and exception handling

## Stream requests

Stream requests return `IAsyncEnumerable<TResponse>`, useful for pagination, data export, or Server-Sent Events.

### Definition

```csharp
public sealed record ListProductsStreamQuery(int PageSize) : IStreamRequest<ProductDto>;

public sealed record ProductDto(int Id, string Name);

public sealed class ListProductsStreamHandler : IStreamRequestHandler<ListProductsStreamQuery, ProductDto>
{
    public async IAsyncEnumerable<ProductDto> Handle(
        ListProductsStreamQuery request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        for (var i = 1; i <= request.PageSize; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return new ProductDto(i, $"Product {i}");
            await Task.Delay(10, cancellationToken);
        }
    }
}
```

### Consumption

```csharp
await foreach (var product in mediator.CreateStream(new ListProductsStreamQuery(10), cancellationToken))
{
    Console.WriteLine(product.Name);
}
```

## Exception handlers

Exception handlers let you define a **fallback response** when the handler throws:

```csharp
public sealed class NotFoundExceptionHandler
    : IRequestExceptionHandler<GetUserQuery, UserDto, KeyNotFoundException>
{
    public Task Handle(
        GetUserQuery request,
        KeyNotFoundException exception,
        RequestExceptionHandlerState<UserDto> state,
        CancellationToken cancellationToken)
    {
        state.Handled = true;
        state.Response = new UserDto(0, "Unknown");
        return Task.CompletedTask;
    }
}
```

When `state.Handled = true`, DotRMediator returns `state.Response` instead of propagating the exception.

Handlers are tried in registration order until one sets `Handled = true`.

## Exception actions

Exception actions run side effects (logging, metrics) **without** replacing the response:

```csharp
public sealed class LogExceptionAction<TRequest, TException> : IRequestExceptionAction<TRequest, TException>
    where TRequest : notnull
    where TException : Exception
{
    private readonly ILogger<LogExceptionAction<TRequest, TException>> _logger;

    public LogExceptionAction(ILogger<LogExceptionAction<TRequest, TException>> logger)
        => _logger = logger;

    public Task Execute(TRequest request, TException exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Error processing {Request}", typeof(TRequest).Name);
        return Task.CompletedTask;
    }
}
```

Actions run **before** exception handlers. If no handler handles the exception, it is rethrown.

## Exception flow

```
Request
  └─ Pipeline Behaviors
       └─ Handler throws
            ├─ Exception Actions (logging, metrics)
            └─ Exception Handlers (optional fallback)
                 ├─ Handled = true → return Response
                 └─ Handled = false → rethrow
```

## OperationCanceledException

Cancellation exceptions (`OperationCanceledException`) are **not** intercepted by exception behaviors, preserving the default cooperative cancellation behavior in .NET.

## Complete example

```csharp
public sealed record RiskyQuery(int Id) : IRequest<string>;

public sealed class RiskyQueryHandler : IRequestHandler<RiskyQuery, string>
{
    public Task<string> Handle(RiskyQuery request, CancellationToken cancellationToken)
        => throw new InvalidOperationException("Service unavailable.");
}

public sealed class RiskyQueryExceptionHandler
    : IRequestExceptionHandler<RiskyQuery, string, InvalidOperationException>
{
    public Task Handle(
        RiskyQuery request,
        InvalidOperationException exception,
        RequestExceptionHandlerState<string> state,
        CancellationToken cancellationToken)
    {
        state.Handled = true;
        state.Response = "fallback-response";
        return Task.CompletedTask;
    }
}

// Usage:
var result = await mediator.Send(new RiskyQuery(1)); // "fallback-response"
```

## Registration

Exception handlers and actions are discovered automatically by assembly scanning when they implement the corresponding generic interfaces.
