using DotRMediator.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DotRMediator.Tests;

public sealed class ExceptionHandlingTests
{
    [Fact]
    public async Task ExceptionHandler_ShouldReturnFallbackResponse()
    {
        var services = new ServiceCollection();
        services.AddDotRMediator(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ExceptionHandlingTests).Assembly)
        );
        var mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();

        var response = await mediator.Send(new FailingRequest(ShouldFail: true));

        Assert.Equal("fallback", response);
    }

    [Fact]
    public async Task ExceptionAction_ShouldExecuteBeforeRethrowing()
    {
        ExceptionActionState.Reset();

        var services = new ServiceCollection();
        services.AddDotRMediator(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ExceptionHandlingTests).Assembly)
        );
        var mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            mediator.Send(new ActionFailingRequest())
        );

        Assert.True(ExceptionActionState.WasExecuted);
    }

    public sealed record FailingRequest(bool ShouldFail) : IRequest<string>;

    public sealed class FailingRequestHandler : IRequestHandler<FailingRequest, string>
    {
        public Task<string> Handle(FailingRequest request, CancellationToken cancellationToken)
        {
            if (request.ShouldFail)
            {
                throw new InvalidOperationException("Simulated failure.");
            }

            return Task.FromResult("ok");
        }
    }

    public sealed class FailingRequestExceptionHandler
        : IRequestExceptionHandler<FailingRequest, string, InvalidOperationException>
    {
        public Task Handle(
            FailingRequest request,
            InvalidOperationException exception,
            RequestExceptionHandlerState<string> state,
            CancellationToken cancellationToken
        )
        {
            state.Handled = true;
            state.Response = "fallback";
            return Task.CompletedTask;
        }
    }

    public sealed record ActionFailingRequest : IRequest<string>;

    public sealed class ActionFailingRequestHandler : IRequestHandler<ActionFailingRequest, string>
    {
        public Task<string> Handle(
            ActionFailingRequest request,
            CancellationToken cancellationToken
        ) => throw new InvalidOperationException("Simulated failure.");
    }

    public sealed class ActionFailingRequestExceptionAction
        : IRequestExceptionAction<ActionFailingRequest, InvalidOperationException>
    {
        public Task Execute(
            ActionFailingRequest request,
            InvalidOperationException exception,
            CancellationToken cancellationToken
        )
        {
            ExceptionActionState.WasExecuted = true;
            return Task.CompletedTask;
        }
    }

    private static class ExceptionActionState
    {
        public static bool WasExecuted { get; set; }

        public static void Reset() => WasExecuted = false;
    }
}
