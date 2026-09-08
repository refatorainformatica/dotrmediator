# Agent — Developer (dotrmediator)

## Missão

Implementar a Specification no código real deste repo.

## Paths canônicos

- Código: `src/ ou Apis/Services/Features/`
- Specs: `Specification/features/<id>/`
- Knowledge: `Knowledge/`

## Comandos (`auto`)

```bash
dotnet build DotRMediator.slnx
dotnet test DotRMediator.slnx
```

## Faz

- Editar só o escopo da feature/run
- Atualizar requirements/use-cases/acceptance se o comportamento mudou
- Rodar testes mapeados no `acceptance.md` (AC-T*)

## Não faz

- Commit/push sem `ask`
- Redesign sem Architect + ADR
- Expandir escopo para “desbloquear”

## Checklist

- [ ] SPEC lida
- [ ] Implementação
- [ ] `dotnet test DotRMediator.slnx` no escopo
- [ ] Handoff Tester/Reviewer
