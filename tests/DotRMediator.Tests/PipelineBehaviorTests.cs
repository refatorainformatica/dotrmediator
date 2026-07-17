using DotRMediator.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DotRMediator.Tests;

public sealed class PipelineBehaviorTests
{
    [Fact]
    public async Task PipelineBehaviors_ShouldExecuteInCorrectOrder()
    {
        PipelineState.Reset();

        var services = new ServiceCollection();
        services.AddDotRMediator(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(PipelineBehaviorTests).Assembly);
            cfg.AddOpenBehavior(typeof(OuterBehavior<,>));
            cfg.AddOpenBehavior(typeof(InnerBehavior<,>));
        });

        var mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();
        await mediator.Send(new OrderedRequest());

        Assert.Equal(
            ["OuterBefore", "InnerBefore", "Handler", "InnerAfter", "OuterAfter"],
            PipelineState.Events
        );
    }

    public sealed record OrderedRequest : IRequest<string>;

    public sealed class OrderedRequestHandler : IRequestHandler<OrderedRequest, string>
    {
        public Task<string> Handle(OrderedRequest request, CancellationToken cancellationToken)
        {
            PipelineState.Events.Add("Handler");
            return Task.FromResult("ok");
        }
    }

    public sealed class OuterBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            PipelineState.Events.Add("OuterBefore");
            var response = await next();
            PipelineState.Events.Add("OuterAfter");
            return response;
        }
    }

    public sealed class InnerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            PipelineState.Events.Add("InnerBefore");
            var response = await next();
            PipelineState.Events.Add("InnerAfter");
            return response;
        }
    }

    private static class PipelineState
    {
        public static List<string> Events { get; } = [];

        public static void Reset() => Events.Clear();
    }
}
