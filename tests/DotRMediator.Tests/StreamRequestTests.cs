using DotRMediator.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DotRMediator.Tests;

public sealed class StreamRequestTests
{
    [Fact]
    public async Task CreateStream_ShouldReturnHandlerItems()
    {
        var services = new ServiceCollection();
        services.AddDotRMediator(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(StreamRequestTests).Assembly)
        );
        var mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();

        var items = new List<int>();
        await foreach (var item in mediator.CreateStream(new NumberStreamRequest(3)))
        {
            items.Add(item);
        }

        Assert.Equal([1, 2, 3], items);
    }

    [Fact]
    public async Task StreamPipelineBehavior_ShouldInterceptStream()
    {
        StreamBehaviorState.Reset();

        var services = new ServiceCollection();
        services.AddDotRMediator(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(StreamRequestTests).Assembly);
            cfg.AddOpenStreamBehavior(typeof(StreamLoggingBehavior<,>));
        });

        var mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();

        await foreach (var _ in mediator.CreateStream(new NumberStreamRequest(2))) { }

        Assert.Equal(["StreamBefore", "StreamAfter"], StreamBehaviorState.Events);
    }

    public sealed record NumberStreamRequest(int Count) : IStreamRequest<int>;

    public sealed class NumberStreamRequestHandler : IStreamRequestHandler<NumberStreamRequest, int>
    {
        public async IAsyncEnumerable<int> Handle(
            NumberStreamRequest request,
            [System.Runtime.CompilerServices.EnumeratorCancellation]
                CancellationToken cancellationToken
        )
        {
            for (var i = 1; i <= request.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return i;
                await Task.Yield();
            }
        }
    }

    public sealed class StreamLoggingBehavior<TRequest, TResponse>
        : IStreamPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async IAsyncEnumerable<TResponse> Handle(
            TRequest request,
            StreamHandlerDelegate<TResponse> next,
            [System.Runtime.CompilerServices.EnumeratorCancellation]
                CancellationToken cancellationToken
        )
        {
            StreamBehaviorState.Events.Add("StreamBefore");

            await foreach (var item in next().WithCancellation(cancellationToken))
            {
                yield return item;
            }

            StreamBehaviorState.Events.Add("StreamAfter");
        }
    }

    private static class StreamBehaviorState
    {
        public static List<string> Events { get; } = [];

        public static void Reset() => Events.Clear();
    }
}
