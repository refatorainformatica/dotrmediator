# Domain — dotrmediator

> Glossário enriquecido em 2026-08-24 a partir de features + código.

## Glossário

| Termo | Significado | Origem |
|-------|-------------|--------|
| Harness | Estrutura `.ai-harness/` deste repositório | `—` |
| Feature | Unidade em `Specification/features/<id>/` | `—` |
| Run | Execução do agente em `Runtime/state/runs/` | `—` |
| Acceptance | Critérios derivados de testes unitários (AC-T*) | `—` |
| dotrmediator | Projeto DotRMediator | `src/DotRMediator` |
| with | Sends a request and awaits a response | `src/with` |
| response | Returns `Unit` | `src/response` |
| without | Returns `Unit` | `src/without` |
| processing | Hooks before/after the handler | `src/processing` |
| Mediator | class em `src/DotRMediator/Mediator.cs` | `src/DotRMediator/Mediator.cs` |
| DotRMediatorServiceConfiguration | class em `src/DotRMediator/DependencyInjection/ServiceCollectionExtensions.cs` | `src/DotRMediator/DependencyInjection/ServiceCollectionExtensions.cs` |
| ServiceCollectionExtensions | class em `src/DotRMediator/DependencyInjection/ServiceCollectionExtensions.cs` | `src/DotRMediator/DependencyInjection/ServiceCollectionExtensions.cs` |
| RequestExceptionActionProcessorBehavior | class em `src/DotRMediator/Behaviors/RequestExceptionActionProcessorBehavior.cs` | `src/DotRMediator/Behaviors/RequestExceptionActionProcessorBehavior.cs` |
| RequestPostProcessorBehavior | class em `src/DotRMediator/Behaviors/RequestPostProcessorBehavior.cs` | `src/DotRMediator/Behaviors/RequestPostProcessorBehavior.cs` |
| RequestPreProcessorBehavior | class em `src/DotRMediator/Behaviors/RequestPreProcessorBehavior.cs` | `src/DotRMediator/Behaviors/RequestPreProcessorBehavior.cs` |
| RequestExceptionProcessorBehavior | class em `src/DotRMediator/Behaviors/RequestExceptionProcessorBehavior.cs` | `src/DotRMediator/Behaviors/RequestExceptionProcessorBehavior.cs` |
| RequestPostProcessor | interface em `src/DotRMediator/Abstractions/IRequestPostProcessor.cs` | `src/DotRMediator/Abstractions/IRequestPostProcessor.cs` |
| RequestExceptionAction | interface em `src/DotRMediator/Abstractions/IRequestExceptionAction.cs` | `src/DotRMediator/Abstractions/IRequestExceptionAction.cs` |
| Notification | interface em `src/DotRMediator/Abstractions/INotification.cs` | `src/DotRMediator/Abstractions/INotification.cs` |
| NotificationHandler | interface em `src/DotRMediator/Abstractions/INotificationHandler.cs` | `src/DotRMediator/Abstractions/INotificationHandler.cs` |
| PipelineBehavior | interface em `src/DotRMediator/Abstractions/IPipelineBehavior.cs` | `src/DotRMediator/Abstractions/IPipelineBehavior.cs` |
| Publisher | interface em `src/DotRMediator/Abstractions/IPublisher.cs` | `src/DotRMediator/Abstractions/IPublisher.cs` |
| RequestHandler | interface em `src/DotRMediator/Abstractions/IRequestHandler.cs` | `src/DotRMediator/Abstractions/IRequestHandler.cs` |
| RequestExceptionHandlerState | class em `src/DotRMediator/Abstractions/RequestExceptionHandlerState.cs` | `src/DotRMediator/Abstractions/RequestExceptionHandlerState.cs` |
| RequestPreProcessor | interface em `src/DotRMediator/Abstractions/IRequestPreProcessor.cs` | `src/DotRMediator/Abstractions/IRequestPreProcessor.cs` |
| StreamRequestHandler | interface em `src/DotRMediator/Abstractions/IStreamRequestHandler.cs` | `src/DotRMediator/Abstractions/IStreamRequestHandler.cs` |
| StreamRequest | interface em `src/DotRMediator/Abstractions/IStreamRequest.cs` | `src/DotRMediator/Abstractions/IStreamRequest.cs` |
| Request | interface em `src/DotRMediator/Abstractions/IRequest.cs` | `src/DotRMediator/Abstractions/IRequest.cs` |
| StreamPipelineBehavior | interface em `src/DotRMediator/Abstractions/IStreamPipelineBehavior.cs` | `src/DotRMediator/Abstractions/IStreamPipelineBehavior.cs` |
| Sender | interface em `src/DotRMediator/Abstractions/ISender.cs` | `src/DotRMediator/Abstractions/ISender.cs` |
| RequestExceptionHandler | interface em `src/DotRMediator/Abstractions/IRequestExceptionHandler.cs` | `src/DotRMediator/Abstractions/IRequestExceptionHandler.cs` |
| RequestHandlerBase | class em `src/DotRMediator/Internal/RequestHandlerWrapper.cs` | `src/DotRMediator/Internal/RequestHandlerWrapper.cs` |
| RequestHandlerWrapper | class em `src/DotRMediator/Internal/RequestHandlerWrapper.cs` | `src/DotRMediator/Internal/RequestHandlerWrapper.cs` |
| StreamHandlerBase | class em `src/DotRMediator/Internal/RequestHandlerWrapper.cs` | `src/DotRMediator/Internal/RequestHandlerWrapper.cs` |
| StreamHandlerWrapper | class em `src/DotRMediator/Internal/RequestHandlerWrapper.cs` | `src/DotRMediator/Internal/RequestHandlerWrapper.cs` |
| RequestHandlerWrapperCache | class em `src/DotRMediator/Internal/RequestHandlerWrapperCache.cs` | `src/DotRMediator/Internal/RequestHandlerWrapperCache.cs` |
| RequestTypeHelper | class em `src/DotRMediator/Internal/RequestHandlerWrapperCache.cs` | `src/DotRMediator/Internal/RequestHandlerWrapperCache.cs` |
| ServiceProviderExtensions | class em `src/DotRMediator/Internal/RequestHandlerWrapperCache.cs` | `src/DotRMediator/Internal/RequestHandlerWrapperCache.cs` |

## Como manter

1. Novo conceito de domínio → linha neste glossário **na mesma PR**.
2. Mesmo identificador em SPEC, código e testes.
3. ADR se o termo implica trade-off arquitetural.
