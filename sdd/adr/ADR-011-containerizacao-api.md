# ADR-011 — Containerização da API com Docker Compose

**Status:** Aceita pelo pedido de implementação em 2026-11-12.

## Contexto

O FinTrack não tinha artefatos para executar a API de modo reproduzível em
contêiner. A persistência SQLite requer armazenamento durável e as migrations,
por ADR-004, não podem ser aplicadas automaticamente no startup da API.

## Decisão

- Criar uma imagem Docker multiestágio para `FinTrack.Infrastructure`, com build
  e publicação .NET 10 separados da imagem final de runtime.
- Criar `docker-compose.yml` com os serviços `migracoes` e `api`.
- Executar `dotnet ef database update` no serviço efêmero `migracoes`, antes da
  API, e manter a API sem migrations no startup.
- Persistir SQLite no volume nomeado `fintrack-dados`.
- Publicar a API na porta host `8080`; usar ambiente `Production` no Compose.
- Manter Scalar/OpenAPI indisponíveis no contêiner de produção, conforme ADR-010.

## Alternativas consideradas

- `Database.Migrate()` no startup: rejeitada pela ADR-004, pois acopla alteração
  de esquema à inicialização e introduz riscos operacionais e de concorrência.
- SQLite dentro do sistema de arquivos efêmero do contêiner: rejeitada por perder
  dados quando o contêiner é recriado.
- Incluir a SPA Angular na mesma imagem: adiada; a ADR-009 define hospedagem da
  SPA como decisão independente.

## Consequências e impacto

- Docker é infraestrutura de execução e não adiciona referência ao Core.
- `docker compose up --build` aplica migrations e inicia a API com dados
  persistidos em volume local.
- O fluxo é adequado ao ambiente local; publicação remota, segredos e hardening
  operacional exigem decisão específica.
