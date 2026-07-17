using System.Reflection;
using DotRMediator.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotRMediator.DependencyInjection;

/// <summary>
/// Configuration options for DotRMediator service registration.
/// </summary>
/// <remarks>
/// Passed to the configure callback of <see cref="ServiceCollectionExtensions.AddDotRMediator"/>.
/// Collects assemblies, behaviors, and service lifetime before registration is applied.
/// </remarks>
public sealed class DotRMediatorServiceConfiguration
{
    /// <summary>
    /// Assemblies scanned for handler and processor implementations.
    /// </summary>
    /// <remarks>
    /// Populated by <see cref="RegisterServicesFromAssembly"/> and <see cref="RegisterServicesFromAssemblyContaining{T}"/>.
    /// </remarks>
    internal List<Assembly> Assemblies { get; } = [];

    /// <summary>
    /// Open generic request pipeline behaviors to register.
    /// </summary>
    /// <remarks>
    /// Each type must implement <see cref="IPipelineBehavior{TRequest, TResponse}"/> as an open generic.
    /// </remarks>
    internal List<Type> OpenBehaviors { get; } = [];

    /// <summary>
    /// Open generic stream pipeline behaviors to register.
    /// </summary>
    /// <remarks>
    /// Each type must implement <see cref="IStreamPipelineBehavior{TRequest, TResponse}"/> as an open generic.
    /// </remarks>
    internal List<Type> OpenStreamBehaviors { get; } = [];

    /// <summary>
    /// Closed request pipeline behaviors to register.
    /// </summary>
    /// <remarks>
    /// Populated by <see cref="AddBehavior{TBehavior}"/>.
    /// </remarks>
    internal List<Type> ClosedBehaviors { get; } = [];

    /// <summary>
    /// Closed stream pipeline behaviors to register.
    /// </summary>
    /// <remarks>
    /// Populated by <see cref="AddStreamBehavior{TBehavior}"/>.
    /// </remarks>
    internal List<Type> ClosedStreamBehaviors { get; } = [];

    /// <summary>
    /// Lifetime of registered mediator and handler services.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="ServiceLifetime.Transient"/>.
    /// </remarks>
    public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;

    /// <summary>
    /// Registers handlers and processors from an assembly.
    /// </summary>
    /// <remarks>
    /// Scans concrete types implementing mediator handler interfaces.
    /// May be called multiple times to include handlers from several assemblies.
    /// </remarks>
    /// <param name="assembly">The assembly to scan.</param>
    /// <returns>This configuration instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="assembly"/> is <c>null</c>.</exception>
    public DotRMediatorServiceConfiguration RegisterServicesFromAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        Assemblies.Add(assembly);
        return this;
    }

    /// <summary>
    /// Registers handlers and processors from a marker type's assembly.
    /// </summary>
    /// <remarks>
    /// Convenience wrapper around <see cref="RegisterServicesFromAssembly"/> using <c>typeof(T).Assembly</c>.
    /// </remarks>
    /// <typeparam name="T">A type defined in the assembly to scan.</typeparam>
    /// <returns>This configuration instance for chaining.</returns>
    public DotRMediatorServiceConfiguration RegisterServicesFromAssemblyContaining<T>() =>
        RegisterServicesFromAssembly(typeof(T).Assembly);

    /// <summary>
    /// Adds an open generic pipeline behavior (e.g. <c>typeof(LoggingBehavior&lt;,&gt;)</c>).
    /// </summary>
    /// <remarks>
    /// The behavior is registered for all closed <see cref="IPipelineBehavior{TRequest, TResponse}"/> type pairs.
    /// </remarks>
    /// <param name="behaviorType">The open generic behavior type.</param>
    /// <returns>This configuration instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="behaviorType"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="behaviorType"/> is not a valid open generic behavior.</exception>
    public DotRMediatorServiceConfiguration AddOpenBehavior(Type behaviorType)
    {
        ValidateOpenGenericBehavior(behaviorType, typeof(IPipelineBehavior<,>));
        OpenBehaviors.Add(behaviorType);
        return this;
    }

    /// <summary>
    /// Adds a closed pipeline behavior.
    /// </summary>
    /// <remarks>
    /// Use when the behavior applies to specific request/response type pairs only.
    /// </remarks>
    /// <typeparam name="TBehavior">The closed behavior implementation type.</typeparam>
    /// <returns>This configuration instance for chaining.</returns>
    public DotRMediatorServiceConfiguration AddBehavior<TBehavior>()
        where TBehavior : class
    {
        ClosedBehaviors.Add(typeof(TBehavior));
        return this;
    }

    /// <summary>
    /// Adds an open generic stream pipeline behavior.
    /// </summary>
    /// <remarks>
    /// The behavior is registered for all closed <see cref="IStreamPipelineBehavior{TRequest, TResponse}"/> type pairs.
    /// </remarks>
    /// <param name="behaviorType">The open generic stream behavior type.</param>
    /// <returns>This configuration instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="behaviorType"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="behaviorType"/> is not a valid open generic stream behavior.</exception>
    public DotRMediatorServiceConfiguration AddOpenStreamBehavior(Type behaviorType)
    {
        ValidateOpenGenericBehavior(behaviorType, typeof(IStreamPipelineBehavior<,>));
        OpenStreamBehaviors.Add(behaviorType);
        return this;
    }

    /// <summary>
    /// Adds a closed stream pipeline behavior.
    /// </summary>
    /// <remarks>
    /// Use when the behavior applies to specific stream request/response type pairs only.
    /// </remarks>
    /// <typeparam name="TBehavior">The closed stream behavior implementation type.</typeparam>
    /// <returns>This configuration instance for chaining.</returns>
    public DotRMediatorServiceConfiguration AddStreamBehavior<TBehavior>()
        where TBehavior : class
    {
        ClosedStreamBehaviors.Add(typeof(TBehavior));
        return this;
    }

    /// <summary>
    /// Validates that a type is an open generic implementation of the expected behavior interface.
    /// </summary>
    /// <remarks>
    /// Shared by <see cref="AddOpenBehavior"/> and <see cref="AddOpenStreamBehavior"/>.
    /// </remarks>
    /// <param name="behaviorType">The behavior type to validate.</param>
    /// <param name="expectedGenericDefinition">The expected open generic behavior interface.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="behaviorType"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when validation fails.</exception>
    private static void ValidateOpenGenericBehavior(
        Type behaviorType,
        Type expectedGenericDefinition
    )
    {
        ArgumentNullException.ThrowIfNull(behaviorType);

        if (!behaviorType.IsGenericTypeDefinition)
        {
            throw new ArgumentException(
                "The behavior must be an open generic type.",
                nameof(behaviorType)
            );
        }

        var implements = behaviorType
            .GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == expectedGenericDefinition);

        if (!implements)
        {
            throw new ArgumentException(
                $"The type '{behaviorType.FullName}' does not implement {expectedGenericDefinition.Name}.",
                nameof(behaviorType)
            );
        }
    }
}

/// <summary>
/// Extension methods to register DotRMediator with the DI container.
/// </summary>
/// <remarks>
/// Entry point for application startup configuration.
/// Registers the mediator, scans assemblies, and wires built-in pipeline behaviors.
/// </remarks>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds DotRMediator and registers handlers from the provided configuration.
    /// </summary>
    /// <remarks>
    /// Registers <see cref="IMediator"/>, <see cref="ISender"/>, and <see cref="IPublisher"/> with the configured lifetime.
    /// Built-in pre/post processor and exception behaviors are always registered.
    /// </remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configure">Callback that configures assemblies and optional behaviors.</param>
    /// <returns>The same service collection for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is <c>null</c>.</exception>
    public static IServiceCollection AddDotRMediator(
        this IServiceCollection services,
        Action<DotRMediatorServiceConfiguration> configure
    )
    {
        ArgumentNullException.ThrowIfNull(configure);

        var configuration = new DotRMediatorServiceConfiguration();
        configure(configuration);

        services.TryAdd(
            new ServiceDescriptor(typeof(IMediator), typeof(Mediator), configuration.Lifetime)
        );
        services.TryAdd(
            new ServiceDescriptor(
                typeof(ISender),
                static sp => sp.GetRequiredService<IMediator>(),
                configuration.Lifetime
            )
        );
        services.TryAdd(
            new ServiceDescriptor(
                typeof(IPublisher),
                static sp => sp.GetRequiredService<IMediator>(),
                configuration.Lifetime
            )
        );

        foreach (var assembly in configuration.Assemblies)
        {
            RegisterHandlersFromAssembly(services, assembly, configuration.Lifetime);
        }

        RegisterBehavior(services, typeof(RequestPreProcessorBehavior<,>), configuration.Lifetime);
        RegisterBehavior(services, typeof(RequestPostProcessorBehavior<,>), configuration.Lifetime);
        RegisterBehavior(
            services,
            typeof(RequestExceptionActionProcessorBehavior<,>),
            configuration.Lifetime
        );
        RegisterBehavior(
            services,
            typeof(RequestExceptionProcessorBehavior<,>),
            configuration.Lifetime
        );

        foreach (var behavior in configuration.OpenBehaviors)
        {
            RegisterBehavior(services, behavior, configuration.Lifetime);
        }

        foreach (var behavior in configuration.ClosedBehaviors)
        {
            services.TryAddEnumerable(
                new ServiceDescriptor(
                    typeof(IPipelineBehavior<,>),
                    behavior,
                    configuration.Lifetime
                )
            );
        }

        foreach (var behavior in configuration.OpenStreamBehaviors)
        {
            RegisterStreamBehavior(services, behavior, configuration.Lifetime);
        }

        foreach (var behavior in configuration.ClosedStreamBehaviors)
        {
            services.TryAddEnumerable(
                new ServiceDescriptor(
                    typeof(IStreamPipelineBehavior<,>),
                    behavior,
                    configuration.Lifetime
                )
            );
        }

        return services;
    }

    /// <summary>
    /// Scans an assembly and registers concrete handler and processor implementations.
    /// </summary>
    /// <remarks>
    /// Matches types implementing known mediator handler interfaces and registers them with <c>TryAddEnumerable</c>.
    /// </remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="assembly">The assembly to scan.</param>
    /// <param name="lifetime">The service lifetime to apply to discovered types.</param>
    private static void RegisterHandlersFromAssembly(
        IServiceCollection services,
        Assembly assembly,
        ServiceLifetime lifetime
    )
    {
        var handlerInterfaces = new[]
        {
            typeof(IRequestHandler<,>),
            typeof(IRequestHandler<>),
            typeof(INotificationHandler<>),
            typeof(IStreamRequestHandler<,>),
            typeof(IRequestPreProcessor<>),
            typeof(IRequestPostProcessor<,>),
            typeof(IRequestExceptionHandler<,,>),
            typeof(IRequestExceptionAction<,>),
        };

        var concreteTypes = assembly
            .GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false });

        foreach (var type in concreteTypes)
        {
            foreach (var serviceType in type.GetInterfaces())
            {
                if (!serviceType.IsGenericType)
                {
                    continue;
                }

                var genericDefinition = serviceType.GetGenericTypeDefinition();

                if (!handlerInterfaces.Contains(genericDefinition))
                {
                    continue;
                }

                services.TryAddEnumerable(new ServiceDescriptor(serviceType, type, lifetime));
            }
        }
    }

    /// <summary>
    /// Registers an open generic request pipeline behavior.
    /// </summary>
    /// <remarks>
    /// Uses <c>TryAddEnumerable</c> so duplicate registrations are ignored.
    /// </remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="openGenericBehavior">The open generic behavior type.</param>
    /// <param name="lifetime">The service lifetime to apply.</param>
    private static void RegisterBehavior(
        IServiceCollection services,
        Type openGenericBehavior,
        ServiceLifetime lifetime
    )
    {
        services.TryAddEnumerable(
            new ServiceDescriptor(typeof(IPipelineBehavior<,>), openGenericBehavior, lifetime)
        );
    }

    /// <summary>
    /// Registers an open generic stream pipeline behavior.
    /// </summary>
    /// <remarks>
    /// Uses <c>TryAddEnumerable</c> so duplicate registrations are ignored.
    /// </remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="openGenericBehavior">The open generic stream behavior type.</param>
    /// <param name="lifetime">The service lifetime to apply.</param>
    private static void RegisterStreamBehavior(
        IServiceCollection services,
        Type openGenericBehavior,
        ServiceLifetime lifetime
    )
    {
        services.TryAddEnumerable(
            new ServiceDescriptor(typeof(IStreamPipelineBehavior<,>), openGenericBehavior, lifetime)
        );
    }
}
