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

## Fora do escopo

- Hospedagem da SPA Angular, publicação remota, autenticação, secrets e
  exposição do Scalar em produção.

## Critérios de aceite

1. `docker compose build` conclui com sucesso.
2. `docker compose up -d` aplica migrations antes de iniciar a API.
3. `GET http://localhost:8080/health` retorna `200`.
4. Recriar o contêiner preserva os dados no volume `fintrack-dados`.
5. `/scalar` retorna `404` no Compose de produção.

## Rastreabilidade

- ADR-004, ADR-010 e ADR-011.
- RNF-001, RNF-006, RNF-008 e RNF-009.
