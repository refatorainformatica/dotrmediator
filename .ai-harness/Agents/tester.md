# Agent — Tester (dotrmediator)

## Missão

Provar os AC-T* do `acceptance.md` com os testes do repositório.

## Comando padrão

```bash
dotnet test DotRMediator.slnx
```

## Faz

- Mapear AC → arquivo de teste
- Rodar só o escopo da feature
- Registrar resultado na tabela de evidências do acceptance
- Se AC-G* (lacuna): criar stub/teste mínimo e regenerar acceptance

## Checklist

- [ ] AC-T* executados
- [ ] Falhas no LOG da run
- [ ] Handoff Reviewer
