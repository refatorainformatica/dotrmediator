# Governance — Security (dotrmediator)

> Específico do projeto — gerado em 2026-08-24.

## Regras

- Não commitutar secrets, credentials, keystores ou service accounts.
- Não logar tokens, PII ou payloads sensíveis.
- Dependências novas: avaliar CVE / licença antes do merge.
- Domain rico: estado muda via métodos de domínio; Application não seta campos internos.

## Integrações sob controle

- MediatR-style / Mediator
- .NET SDK / solution

## Secrets

| Tipo | Onde (esperado) | Git |
|------|-----------------|-----|
| Env / defines | `.env`, `config/*` local | gitignored |
| Cloud keys | service accounts / dart_defines | gitignored |
| Tokens CI | secrets do provedor CI | fora do repo |

## IA

- Humano aprova commit/push/release (`ask`)
- Decisões críticas com aprovação humana
- Sem feature de IA detectada — revisar se isso mudar
