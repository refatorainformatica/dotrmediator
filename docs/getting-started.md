# Getting started

This guide shows how to integrate DotRMediator into an ASP.NET Core application.

## Prerequisites

- .NET 8 SDK or later
- Basic knowledge of dependency injection

## Step 1 — Add a package reference

```bash
dotnet add reference ../DotRMediator/DotRMediator.csproj
```

## Step 2 — Model commands and queries

```csharp
// Command without a return value
public sealed record CreateOrderCommand(string Product, int Quantity) : IRequest;

// Query with a return value
public sealed record GetOrderStatusQuery(Guid OrderId) : IRequest<OrderStatusDto>;

public sealed record OrderStatusDto(Guid OrderId, string Status);
```

## Step 3 — Implement handlers

```csharp
public sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand>
{
    public Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Persist order...
        return Task.CompletedTask;
    }
}

public sealed class GetOrderStatusHandler : IRequestHandler<GetOrderStatusQuery, OrderStatusDto>
{
    public Task<OrderStatusDto> Handle(GetOrderStatusQuery request, CancellationToken cancellationToken)
    {
        var dto = new OrderStatusDto(request.OrderId, "Processing");
        return Task.FromResult(dto);
    }
}
```

## Step 4 — Register in `Program.cs`

```csharp
using DotRMediator.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDotRMediator(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

var app = builder.Build();
```

The `AddDotRMediator` method automatically registers:

| Service | Default lifetime |
|---------|------------------|
| `IMediator` | Transient |
| `ISender` | Transient |
| `IPublisher` | Transient |
| Handlers discovered in the assembly | Transient |

## Step 5 — Consume via `IMediator`

```csharp
public sealed class OrdersEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/orders", async (CreateOrderCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(cmd, ct);
            return Results.Accepted();
        });

        app.MapGet("/orders/{id:guid}", async (Guid id, IMediator mediator, CancellationToken ct) =>
        {
            var status = await mediator.Send(new GetOrderStatusQuery(id), ct);
            return Results.Ok(status);
        });
    }
}
```

## Step 6 — Publish events

```csharp
public sealed record OrderCreatedEvent(Guid OrderId) : INotification;

public sealed class SendEmailHandler : INotificationHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Send email...
        return Task.CompletedTask;
    }
}

// In the command handler:
await _mediator.Publish(new OrderCreatedEvent(orderId), cancellationToken);
```

## Next steps

- Add [pipeline behaviors](./behaviors.md) for validation and logging
- Configure [exception handlers](./streams-and-exceptions.md) for fallback responses
- Explore [stream requests](./streams-and-exceptions.md) for pagination or SSE scenarios
