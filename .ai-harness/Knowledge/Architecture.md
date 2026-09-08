# Architecture — dotrmediator

> Gerado automaticamente em 2026-08-24. Revise e refine.

## Estilo

- Clean Architecture / CQRS (.NET)

## Estrutura (topo do repositório)

```text
dotrmediator/
├── docs/
│   ├── behaviors.md
│   ├── getting-started.md
│   ├── requests-and-notifications.md
│   └── streams-and-exceptions.md
├── src/
│   └── DotRMediator/
│       ├── Abstractions/
│       ├── Behaviors/
│       ├── DependencyInjection/
│       ├── Internal/
│       ├── DotRMediator.csproj
│       ├── Mediator.cs
│       └── Unit.cs
├── tests/
│   └── DotRMediator.Tests/
│       ├── DotRMediator.Tests.csproj
│       ├── ExceptionHandlingTests.cs
│       ├── GlobalUsings.cs
│       ├── MediatorPublishTests.cs
│       ├── MediatorSendTests.cs
│       ├── PipelineBehaviorTests.cs
│       ├── PrePostProcessorTests.cs
│       ├── StreamRequestTests.cs
│       └── UnitTests.cs
├── banner.jpg
├── DotRMediator.slnx
├── LICENSE
└── README.md
```

## Raiz de código principal

`src`

## Stack detectada

| Camada | Tecnologia |
|--------|------------|
| Runtime | .NET (DotRMediator.slnx) |
| Harness | `.ai-harness/` |

## Relação harness ↔ código

| Spec feature | Código |
|--------------|--------|
| `Specification/features/<id>/` | módulo/pasta correspondente |

## Cross-cutting (preencher)

| Peça | Papel |
|------|--------|
| DI | ver README / código |
| Auth | ver README / código |
| Persistência | ver README / código |
| Observability | ver README / código |

## Fora de escopo arquitetural (atual)

- Consultar README.md e issues abertas
