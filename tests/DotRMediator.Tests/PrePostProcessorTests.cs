using DotRMediator.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DotRMediator.Tests;

public sealed class PrePostProcessorTests
{
    [Fact]
    public async Task PrePostProcessors_ShouldExecuteBeforeAndAfterHandler()
    {
        ProcessorState.Reset();

        var services = new ServiceCollection();
        services.AddDotRMediator(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(PrePostProcessorTests).Assembly)
        );
        var mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();

        var response = await mediator.Send(new ProcessRequest("value"));

        Assert.Equal("value-processed", response);
        Assert.Equal(["Pre", "Handler", "Post"], ProcessorState.Events);
    }

    public sealed record ProcessRequest(string Value) : IRequest<string>;

    public sealed class ProcessRequestHandler : IRequestHandler<ProcessRequest, string>
    {
        public Task<string> Handle(ProcessRequest request, CancellationToken cancellationToken)
        {
            ProcessorState.Events.Add("Handler");
            return Task.FromResult($"{request.Value}-processed");
        }
    }

    public sealed class ProcessPreProcessor : IRequestPreProcessor<ProcessRequest>
    {
        public Task Process(ProcessRequest request, CancellationToken cancellationToken)
        {
            ProcessorState.Events.Add("Pre");
            return Task.CompletedTask;
        }
    }

    public sealed class ProcessPostProcessor : IRequestPostProcessor<ProcessRequest, string>
    {
        public Task Process(
            ProcessRequest request,
            string response,
            CancellationToken cancellationToken
        )
        {
            ProcessorState.Events.Add("Post");
            return Task.CompletedTask;
        }
    }

    private static class ProcessorState
    {
        public static List<string> Events { get; } = [];

        public static void Reset() => Events.Clear();
    }
}
