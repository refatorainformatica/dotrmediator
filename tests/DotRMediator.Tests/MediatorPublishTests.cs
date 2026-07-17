using DotRMediator.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DotRMediator.Tests;

public sealed class MediatorPublishTests
{
    [Fact]
    public async Task Publish_ShouldExecuteAllHandlers()
    {
        NotificationState.Reset();

        var services = new ServiceCollection();
        services.AddDotRMediator(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(MediatorPublishTests).Assembly)
        );
        var mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();

        await mediator.Publish(new UserCreatedNotification(42));

        Assert.Equal(2, NotificationState.HandledCount);
        Assert.Contains(42, NotificationState.HandledIds);
    }

    [Fact]
    public async Task Publish_WithDynamicObject_ShouldExecuteHandlers()
    {
        NotificationState.Reset();

        var services = new ServiceCollection();
        services.AddDotRMediator(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(MediatorPublishTests).Assembly)
        );
        var mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();

        object notification = new UserCreatedNotification(99);
        await mediator.Publish(notification);

        Assert.Equal(2, NotificationState.HandledCount);
    }

    [Fact]
    public async Task Publish_WithInvalidObject_ShouldThrowArgumentException()
    {
        var services = new ServiceCollection();
        services.AddDotRMediator(_ => { });
        var mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();

        await Assert.ThrowsAsync<ArgumentException>(() => mediator.Publish(new object()));
    }

    public sealed record UserCreatedNotification(int UserId) : INotification;

    public sealed class EmailNotificationHandler : INotificationHandler<UserCreatedNotification>
    {
        public Task Handle(
            UserCreatedNotification notification,
            CancellationToken cancellationToken
        )
        {
            NotificationState.Register(notification.UserId);
            return Task.CompletedTask;
        }
    }

    public sealed class AuditNotificationHandler : INotificationHandler<UserCreatedNotification>
    {
        public Task Handle(
            UserCreatedNotification notification,
            CancellationToken cancellationToken
        )
        {
            NotificationState.Register(notification.UserId);
            return Task.CompletedTask;
        }
    }

    private static class NotificationState
    {
        public static int HandledCount { get; private set; }
        public static List<int> HandledIds { get; } = [];

        public static void Register(int id)
        {
            HandledCount++;
            HandledIds.Add(id);
        }

        public static void Reset()
        {
            HandledCount = 0;
            HandledIds.Clear();
        }
    }
}
