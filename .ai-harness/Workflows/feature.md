# Workflow — Feature (dotrmediator)

1. Run (`Runtime/RUN.md`) — workflow=`feature`
2. Spec em `Specification/features/<id>/` (criar a partir de templates se nova)
3. Architect se mudar boundaries → ADR
4. Developer implementa em `src/ ou Apis/Services/Features/`
5. Tester: `dotnet test DotRMediator.slnx` conforme acceptance
6. Reviewer: Governance + Knowledge
7. DONE; commit só com `ask`

## DoR

- [ ] requirements + use-cases + acceptance
- [ ] Knowledge lido

## DoD

- [ ] AC-T* verdes (ou AC-G* justificado + stub criado)
- [ ] STATUS=DONE
