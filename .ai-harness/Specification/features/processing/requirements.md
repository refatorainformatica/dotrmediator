# Requirements — processing

| Campo | Valor |
|-------|-------|
| Feature | processing |
| Status | stable |
| Código | `src/processing` |
| Owners | engineering |
| Atualizado | 2026-08-24 |

## Intenção

Hooks before/after the handler

## Requisitos funcionais

1. Hooks before/after the handler

## Requisitos não-funcionais

- Baseline: `Governance/quality.md`, `Governance/security.md`
- Respeitar `Knowledge/Architecture.md` e `Knowledge/Standards.md`
- Alterações exigem evidência em `acceptance.md`

## Regras globais do produto (aplicáveis)

- Ver README.md

## Fora de escopo

- Itens marcados no README como roadmap/limitações

## Dependências

- Knowledge: Architecture, Domain, Standards
- Código: `src/processing`


### Tipos / símbolos do código

Código: `src/processing`

- `Mediator`
- `DotRMediatorServiceConfiguration`
- `ServiceCollectionExtensions`
- `RequestExceptionActionProcessorBehavior`
- `RequestPostProcessorBehavior`

> Atualizado por enrich-harness em 2026-08-24.
