# Use Cases — dotrmediator

> Derivados de testes/código em 2026-08-24.

## UC-01 — PipelineBehaviors_ShouldExecuteInCorrectOrder

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMediator.Tests/PipelineBehaviorTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Pipeline Behaviors
- **Then** should Execute In Correct Order

## UC-02 — Send_ShouldReturnHandlerResponse

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMediator.Tests/MediatorSendTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Send
- **Then** should Return Handler Response

## UC-03 — Send_WithDynamicRequest_ShouldReturnResponse

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMediator.Tests/MediatorSendTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Send With Dynamic Request
- **Then** should Return Response

## UC-04 — Send_WithVoidRequest_ShouldReturnUnit

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMediator.Tests/MediatorSendTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Send With Void Request
- **Then** should Return Unit

## UC-05 — Send_WithoutRegisteredHandler_ShouldThrowInvalidOperationException

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMediator.Tests/MediatorSendTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Send Without Registered Handler
- **Then** Send Without Registered Handler Should Throw Invalid Operation Exception

## UC-06 — Publish_ShouldExecuteAllHandlers

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMediator.Tests/MediatorPublishTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Publish
- **Then** should Execute All Handlers

## UC-07 — Publish_WithDynamicObject_ShouldExecuteHandlers

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMediator.Tests/MediatorPublishTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Publish With Dynamic Object
- **Then** should Execute Handlers

## UC-08 — Publish_WithInvalidObject_ShouldThrowArgumentException

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMediator.Tests/MediatorPublishTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Publish With Invalid Object
- **Then** Publish With Invalid Object Should Throw Argument Exception

## UC-09 — PrePostProcessors_ShouldExecuteBeforeAndAfterHandler

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMediator.Tests/PrePostProcessorTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Pre Post Processors
- **Then** should Execute Before And After Handler

## UC-10 — ExceptionHandler_ShouldReturnFallbackResponse

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMediator.Tests/ExceptionHandlingTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Exception Handler
- **Then** should Return Fallback Response

## UC-11 — ExceptionAction_ShouldExecuteBeforeRethrowing

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMediator.Tests/ExceptionHandlingTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Exception Action
- **Then** Exception Action Should Execute Before Rethrowing

## UC-12 — CreateStream_ShouldReturnHandlerItems

- **Actor:** sistema sob teste
- **Fonte:** `tests/DotRMediator.Tests/StreamRequestTests.cs` (fact)
- **Given** o sistema está no estado inicial do teste
- **When** Create Stream
- **Then** should Return Handler Items
