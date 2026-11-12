# SPEC-F11 — Documentar e explorar a API com Scalar

**Status:** Aprovada pelo pedido de implementação em 2026-11-12.

## Objetivo

Disponibilizar, no ambiente de desenvolvimento, uma documentação OpenAPI da API
FinTrack e uma referência interativa Scalar para consulta e teste manual dos
endpoints.

## Escopo

- Gerar o documento OpenAPI `v1` em `/openapi/v1.json`.
- Publicar a interface Scalar em `/scalar`.
- Manter os dois recursos restritos a `Development`.
- Cobrir as rotas por testes de integração existentes.

## Fora do escopo

- Alterar contratos, regras de negócio, persistência ou endpoints funcionais.
- Expor documentação em produção.
- Configurar autenticação, credenciais, proxies ou servidores remotos no Scalar.

## Critérios de aceite

1. Em `Development`, `GET /openapi/v1.json` retorna `200` e um documento OpenAPI.
2. Em `Development`, `GET /scalar` retorna `200` e a interface Scalar.
3. A API, o Core e seus contratos de negócio não sofrem alterações funcionais.
4. Restore, build e testes da solution são concluídos sem erro.

## Rastreabilidade

- ADR-010.
- RNF-001, RNF-004, RNF-007 e RNF-010.
