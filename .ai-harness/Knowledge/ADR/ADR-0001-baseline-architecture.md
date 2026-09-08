# ADR-0001: Baseline architecture — dotrmediator

| Campo | Valor |
|-------|-------|
| Status | accepted |
| Data | 2026-08-24 |
| Decisores | engineering (inferido do repositório) |

## Context

O repositório `dotrmediator` precisa de uma linha-base arquitetural explícita para agentes e humanos.
Decisões foram inferidas do código, README e dependências em 2026-08-24.

## Decision

- **Estilo:** Clean Architecture / CQRS (.NET)
- **Integrações adotadas:** MediatR-style / Mediator, .NET SDK / solution
- **Harness:** pasta `.ai-harness/` como contrato operacional (Specs + Agents + Governance)
- **Aceite:** critérios de aceitação amarrados a testes unitários existentes (`AC-T*`)

## Alternatives considered

1. Monólito anêmico sem camadas
2. Microserviços prematuros
3. Mediator in-process vs gRPC

## Consequences

### Positivas
- Agentes têm âncora clara (Knowledge + ADR)
- Mudanças estruturais exigem novo ADR
- Testes viram contrato de aceite

### Negativas / riscos
- ADR inferido pode precisar revisão humana
- Integrações listadas refletem o estado atual do código, não o roadmap

## Links

- `Knowledge/Architecture.md`
- `Knowledge/Domain.md`
- README.md
