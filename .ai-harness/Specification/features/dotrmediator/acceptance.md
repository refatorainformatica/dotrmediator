# Acceptance Criteria — dotrmediator

> Gerado a partir dos **testes unitários** do repositório em 2026-08-24.

## Comando de validação (escopo)

```bash
dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj
```

## Critérios (= testes existentes)

- [ ] AC-T01 — `tests/DotRMediator.Tests/PipelineBehaviorTests.cs` :: PipelineBehaviors_ShouldExecuteInCorrectOrder
- [ ] AC-T02 — `tests/DotRMediator.Tests/MediatorSendTests.cs` :: Send_ShouldReturnHandlerResponse
- [ ] AC-T03 — `tests/DotRMediator.Tests/MediatorSendTests.cs` :: Send_WithDynamicRequest_ShouldReturnResponse
- [ ] AC-T04 — `tests/DotRMediator.Tests/MediatorSendTests.cs` :: Send_WithVoidRequest_ShouldReturnUnit
- [ ] AC-T05 — `tests/DotRMediator.Tests/MediatorSendTests.cs` :: Send_WithoutRegisteredHandler_ShouldThrowInvalidOperationException
- [ ] AC-T06 — `tests/DotRMediator.Tests/MediatorPublishTests.cs` :: Publish_ShouldExecuteAllHandlers
- [ ] AC-T07 — `tests/DotRMediator.Tests/MediatorPublishTests.cs` :: Publish_WithDynamicObject_ShouldExecuteHandlers
- [ ] AC-T08 — `tests/DotRMediator.Tests/MediatorPublishTests.cs` :: Publish_WithInvalidObject_ShouldThrowArgumentException
- [ ] AC-T09 — `tests/DotRMediator.Tests/WithHarnessGapTests.cs` :: Feature_With_NeedsUnitCoverage
- [ ] AC-T10 — `tests/DotRMediator.Tests/PrePostProcessorTests.cs` :: PrePostProcessors_ShouldExecuteBeforeAndAfterHandler
- [ ] AC-T11 — `tests/DotRMediator.Tests/ExceptionHandlingTests.cs` :: ExceptionHandler_ShouldReturnFallbackResponse
- [ ] AC-T12 — `tests/DotRMediator.Tests/ExceptionHandlingTests.cs` :: ExceptionAction_ShouldExecuteBeforeRethrowing
- [ ] AC-T13 — `tests/DotRMediator.Tests/StreamRequestTests.cs` :: CreateStream_ShouldReturnHandlerItems
- [ ] AC-T14 — `tests/DotRMediator.Tests/StreamRequestTests.cs` :: StreamPipelineBehavior_ShouldInterceptStream
- [ ] AC-T15 — `tests/DotRMediator.Tests/UnitTests.cs` :: Unit_ShouldEqualAnotherInstance
- [ ] AC-T16 — `tests/DotRMediator.Tests/WithoutHarnessGapTests.cs` :: Feature_Without_NeedsUnitCoverage
- [ ] AC-T17 — `tests/DotRMediator.Tests/ResponseHarnessGapTests.cs` :: Feature_Response_NeedsUnitCoverage
- [ ] AC-T18 — `tests/DotRMediator.Tests/ProcessingHarnessGapTests.cs` :: Feature_Processing_NeedsUnitCoverage

## Evidências

| AC | Teste | Arquivo | Comando | Resultado |
|----|-------|---------|---------|-----------|
| AC-T01 | PipelineBehaviors_ShouldExecuteInCorrectOrder | `tests/DotRMediator.Tests/PipelineBehaviorTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~PipelineBehaviors_ShouldExecuteInCorrectOrder` | pending |
| AC-T02 | Send_ShouldReturnHandlerResponse | `tests/DotRMediator.Tests/MediatorSendTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~Send_ShouldReturnHandlerResponse` | pending |
| AC-T03 | Send_WithDynamicRequest_ShouldReturnResponse | `tests/DotRMediator.Tests/MediatorSendTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~Send_WithDynamicRequest_ShouldReturnResponse` | pending |
| AC-T04 | Send_WithVoidRequest_ShouldReturnUnit | `tests/DotRMediator.Tests/MediatorSendTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~Send_WithVoidRequest_ShouldReturnUnit` | pending |
| AC-T05 | Send_WithoutRegisteredHandler_ShouldThrowInvalidOperationExc | `tests/DotRMediator.Tests/MediatorSendTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~Send_WithoutRegisteredHandler_ShouldThrowInvalidOperationException` | pending |
| AC-T06 | Publish_ShouldExecuteAllHandlers | `tests/DotRMediator.Tests/MediatorPublishTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~Publish_ShouldExecuteAllHandlers` | pending |
| AC-T07 | Publish_WithDynamicObject_ShouldExecuteHandlers | `tests/DotRMediator.Tests/MediatorPublishTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~Publish_WithDynamicObject_ShouldExecuteHandlers` | pending |
| AC-T08 | Publish_WithInvalidObject_ShouldThrowArgumentException | `tests/DotRMediator.Tests/MediatorPublishTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~Publish_WithInvalidObject_ShouldThrowArgumentException` | pending |
| AC-T09 | Feature_With_NeedsUnitCoverage | `tests/DotRMediator.Tests/WithHarnessGapTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~Feature_With_NeedsUnitCoverage` | pending |
| AC-T10 | PrePostProcessors_ShouldExecuteBeforeAndAfterHandler | `tests/DotRMediator.Tests/PrePostProcessorTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~PrePostProcessors_ShouldExecuteBeforeAndAfterHandler` | pending |
| AC-T11 | ExceptionHandler_ShouldReturnFallbackResponse | `tests/DotRMediator.Tests/ExceptionHandlingTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~ExceptionHandler_ShouldReturnFallbackResponse` | pending |
| AC-T12 | ExceptionAction_ShouldExecuteBeforeRethrowing | `tests/DotRMediator.Tests/ExceptionHandlingTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~ExceptionAction_ShouldExecuteBeforeRethrowing` | pending |
| AC-T13 | CreateStream_ShouldReturnHandlerItems | `tests/DotRMediator.Tests/StreamRequestTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~CreateStream_ShouldReturnHandlerItems` | pending |
| AC-T14 | StreamPipelineBehavior_ShouldInterceptStream | `tests/DotRMediator.Tests/StreamRequestTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~StreamPipelineBehavior_ShouldInterceptStream` | pending |
| AC-T15 | Unit_ShouldEqualAnotherInstance | `tests/DotRMediator.Tests/UnitTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~Unit_ShouldEqualAnotherInstance` | pending |
| AC-T16 | Feature_Without_NeedsUnitCoverage | `tests/DotRMediator.Tests/WithoutHarnessGapTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~Feature_Without_NeedsUnitCoverage` | pending |
| AC-T17 | Feature_Response_NeedsUnitCoverage | `tests/DotRMediator.Tests/ResponseHarnessGapTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~Feature_Response_NeedsUnitCoverage` | pending |
| AC-T18 | Feature_Processing_NeedsUnitCoverage | `tests/DotRMediator.Tests/ProcessingHarnessGapTests.cs` | `dotnet test tests/DotRMediator.Tests/DotRMediator.Tests.csproj --filter FullyQualifiedName~Feature_Processing_NeedsUnitCoverage` | pending |

## Critérios de processo

- [ ] AC-P01 — Use cases em `use-cases.md` coerentes com os testes acima
- [ ] AC-P02 — Sem violação de Knowledge / Governance
- [ ] AC-P03 — Novos comportamentos exigem novo teste antes de marcar DONE
