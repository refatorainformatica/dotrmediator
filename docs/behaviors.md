# Pipeline behaviors

Pipeline behaviors let you intercept requests before and after handler execution, similar to HTTP middleware.

## Contract

```csharp
public interface IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken);
}
```

`RequestHandlerDelegate<TResponse>` represents the next step in the pipeline (another behavior or the final handler).

## Example — Logging

```csharp
public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);

        var response = await next();

        _logger.LogInformation("Handled {RequestName}", typeof(TRequest).Name);
        return response;
    }
}
```

## Registration

### Open generic (recommended)

```csharp
services.AddDotRMediator(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
```

### Closed behavior

```csharp
cfg.AddBehavior<MySpecificBehavior>();
```

## Execution order

Behaviors are stacked like a chain of responsibility:

```
Behavior A (registered 1st) ──► Behavior B (registered 2nd) ──► Handler
```

- The **first registered behavior** is the most **outer** (runs first in the "before" phase)
- The **last registered behavior** sits **closest to the handler**

Example with two custom behaviors:

```
Registration: LoggingBehavior, ValidationBehavior

Execution:
  Logging.Before
    Validation.Before
      Handler
    Validation.After
  Logging.After
```

## Built-in behaviors

DotRMediator registers these automatically:

| Behavior | Purpose |
|----------|---------|
| `RequestPreProcessorBehavior<,>` | Runs `IRequestPreProcessor<T>` |
| `RequestPostProcessorBehavior<,>` | Runs `IRequestPostProcessor<T, TResponse>` |
| `RequestExceptionActionProcessorBehavior<,>` | Runs exception actions and rethrows |
| `RequestExceptionProcessorBehavior<,>` | Runs exception handlers with fallback |

## Pre-processors

```csharp
public sealed class ValidateRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        // Validation before the handler...
        return Task.CompletedTask;
    }
}
```

## Post-processors

```csharp
public sealed class AuditPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    where TRequest : notnull
{
    public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        // Audit after the response...
        return Task.CompletedTask;
    }
}
```

## Stream pipeline behaviors

For `IStreamRequest<T>`, use `IStreamPipelineBehavior<TRequest, TResponse>`:

```csharp
public sealed class StreamLoggingBehavior<TRequest, TResponse> : IStreamPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async IAsyncEnumerable<TResponse> Handle(
        TRequest request,
        StreamHandlerDelegate<TResponse> next,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var item in next().WithCancellation(cancellationToken))
        {
            yield return item;
        }
    }
}
```

Registration:

```csharp
cfg.AddOpenStreamBehavior(typeof(StreamLoggingBehavior<,>));
```

## Limitations

- Pipeline behaviors apply to **requests** only, not **notifications**
- For notifications, share logic via base classes or manual decorators
