# Requests and notifications

## Requests (Commands and Queries)

### Request with a response

```csharp
public sealed record GetProductPriceQuery(int ProductId) : IRequest<decimal>;

public sealed class GetProductPriceHandler : IRequestHandler<GetProductPriceQuery, decimal>
{
    public Task<decimal> Handle(GetProductPriceQuery request, CancellationToken cancellationToken)
        => Task.FromResult(99.90m);
}
```

### Request without a response

Requests that return no value implement `IRequest` (equivalent to `IRequest<Unit>`):

```csharp
public sealed record DeleteProductCommand(int ProductId) : IRequest;

public sealed class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
{
    public Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        // Remove product...
        return Task.CompletedTask;
    }
}
```

### Dynamic dispatch

When the request type is only known at runtime:

```csharp
object request = new GetProductPriceQuery(1);
var result = await mediator.Send(request);
var price = (decimal)result!;
```

## Notifications (Events)

Notifications allow **multiple handlers** to react to the same event:

```csharp
public sealed record ProductUpdatedEvent(int ProductId, decimal NewPrice) : INotification;

public sealed class CacheInvalidationHandler : INotificationHandler<ProductUpdatedEvent>
{
    public Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // Invalidate cache...
        return Task.CompletedTask;
    }
}

public sealed class AuditLogHandler : INotificationHandler<ProductUpdatedEvent>
{
    public Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // Write audit log...
        return Task.CompletedTask;
    }
}
```

Notification handlers are executed **sequentially** in container registration order.

## Dispatch interfaces

| Interface | Method | Use case |
|-----------|--------|----------|
| `IMediator` | `Send`, `Publish`, `CreateStream` | Full facade |
| `ISender` | `Send` | Commands and queries only |
| `IPublisher` | `Publish` | Events only |

Restrict dependencies when you want to limit responsibilities:

```csharp
public sealed class OrderService(ISender sender, IPublisher publisher)
{
    public async Task CreateAsync(CreateOrderCommand command, CancellationToken ct)
    {
        await sender.Send(command, ct);
        await publisher.Publish(new OrderCreatedEvent(Guid.NewGuid()), ct);
    }
}
```

## Automatic handler discovery

Assembly scanning registers implementations of:

- `IRequestHandler<TRequest, TResponse>`
- `IRequestHandler<TRequest>`
- `INotificationHandler<TNotification>`
- `IStreamRequestHandler<TRequest, TResponse>`
- `IRequestPreProcessor<TRequest>`
- `IRequestPostProcessor<TRequest, TResponse>`
- `IRequestExceptionHandler<TRequest, TResponse, TException>`
- `IRequestExceptionAction<TRequest, TException>`

Handlers must be concrete (non-abstract) classes in the registered assembly.

## Best practices

1. **One handler per request** — keep handlers small and focused
2. **Immutable records** — prefer `record` for commands, queries, and events
3. **Explicit naming** — `CreateUserCommand`, `GetUserByIdQuery`, `UserCreatedEvent`
4. **Notifications for side effects** — email, cache, audit after the main command
