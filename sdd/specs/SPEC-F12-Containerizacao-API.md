# SPEC-F12 — Executar a API em Docker

**Status:** Aprovada pelo pedido de implementação em 2026-11-12.

## Objetivo

Permitir executar a API FinTrack localmente em Docker, com banco SQLite
persistente e migrations aplicadas de modo explícito antes da API.

## Escopo

- `Dockerfile` multiestágio para a API .NET 10.
- `.dockerignore` para reduzir e isolar o contexto de build.
- `docker-compose.yml` com serviços de migrations e API.
- Volume nomeado para o arquivo SQLite e porta host `8080`.
- A carga única de dados de demonstração é especificada separadamente pela
  SPEC-F13 e executada por serviço efêmero entre migrations e API.

## Fora do escopo

- Hospedagem da SPA Angular, publicação remota, autenticação, secrets e
  exposição do Scalar em produção.

## Critérios de aceite

1. `docker compose build` conclui com sucesso.
2. `docker compose up -d` aplica migrations antes de iniciar a API.
3. `GET http://localhost:8080/health` retorna `200`.
4. Recriar o contêiner preserva os dados no volume `fintrack-dados`.
5. `/scalar` retorna `404` no Compose de produção.

**Nota:** o Compose deste repositório é exclusivamente local e usa
`Development`, disponibilizando `/scalar` e `/openapi/v1.json` conforme
ADR-013. O critério 5 se aplica a deploys configurados como `Production`, não
ao Compose local.

## Rastreabilidade

- ADR-004, ADR-010 e ADR-011.
- A sequência de dados de demonstração é definida por SPEC-F13 e ADR-012.
- RNF-001, RNF-006, RNF-008 e RNF-009.
