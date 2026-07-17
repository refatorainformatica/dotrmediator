using DotRMediator.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DotRMediator.Tests;

public sealed class MediatorSendTests
{
    [Fact]
    public async Task Send_ShouldReturnHandlerResponse()
    {
        var services = new ServiceCollection();
        services.AddDotRMediator(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(MediatorSendTests).Assembly)
        );
        var mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();

        var response = await mediator.Send(new PingRequest("hello"));

        Assert.Equal("hello-pong", response);
    }

    [Fact]
    public async Task Send_WithDynamicRequest_ShouldReturnResponse()
    {
        var services = new ServiceCollection();
        services.AddDotRMediator(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(MediatorSendTests).Assembly)
        );
        var mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();

        object request = new PingRequest("dynamic");
        var response = await mediator.Send(request);

        Assert.Equal("dynamic-pong", response);
    }

    [Fact]
    public async Task Send_WithVoidRequest_ShouldReturnUnit()
    {
        var services = new ServiceCollection();
        services.AddDotRMediator(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(MediatorSendTests).Assembly)
        );
        var mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();

        VoidRequestHandler.Reset();
        var response = await mediator.Send(new VoidRequest("executed"));

        Assert.Equal(Unit.Value, response);
        Assert.True(VoidRequestHandler.WasExecuted);
    }

    [Fact]
    public async Task Send_WithoutRegisteredHandler_ShouldThrowInvalidOperationException()
    {
        var services = new ServiceCollection();
        services.AddDotRMediator(_ => { });
        var mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            mediator.Send(new PingRequest("fail"))
        );
    }

    public sealed record PingRequest(string Message) : IRequest<string>;

    public sealed class PingRequestHandler : IRequestHandler<PingRequest, string>
    {
        public Task<string> Handle(PingRequest request, CancellationToken cancellationToken) =>
            Task.FromResult($"{request.Message}-pong");
    }

    public sealed record VoidRequest(string Message) : IRequest;

    public sealed class VoidRequestHandler : IRequestHandler<VoidRequest>
    {
        public static bool WasExecuted { get; private set; }

        public static void Reset() => WasExecuted = false;

        public Task Handle(VoidRequest request, CancellationToken cancellationToken)
        {
            WasExecuted = true;
            return Task.CompletedTask;
        }
    }
}
