# Governance — Cost (dotrmediator)

> FinOps / custo de compute e tokens — gerado em 2026-08-24.

## Políticas

| CI / build | Rodar testes focados no escopo da feature, não a suíte inteira a cada iteração |
| Agente (loops) | Máx. ~12 iterações/run; evitar rebuilds caros em loop |

## Preferência OSS

1. Biblioteca/modelo open source (local ou self-hosted)
2. API/serviço OSS auto-hospedado
3. Cloud proprietário **somente** com ADR motivando o gap

## Limites sugeridos (ajuste à empresa)

| Recurso | Política |
|---------|----------|
| Cloud SKU | sem upgrade sem `ask` |
| CI minutos | testes do escopo da run |
| Tokens LLM | local primeiro; pago com autorização |
