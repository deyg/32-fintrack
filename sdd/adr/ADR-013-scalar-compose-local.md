# ADR-013 — Scalar no Docker Compose local

**Status:** Aceita em 2026-11-12 por aprovação explícita do usuário.

## Contexto

A ADR-010 limita o mapeamento de Scalar e OpenAPI ao ambiente `Development`.
O Compose local usava `Production`, então `/scalar` e `/openapi/v1.json` não
eram mapeados, embora o usuário quisesse explorar a API local em Docker.

## Decisão

- Configurar somente o serviço `api` do Docker Compose local com
  `ASPNETCORE_ENVIRONMENT=Development`.
- Manter migrations e inicialização de demonstração como etapas anteriores,
  sem alterar a ordem do Compose.
- Manter a publicação de `/scalar` e `/openapi/v1.json` limitada ao ambiente
  `Development`, conforme ADR-010.
- Deploys Production devem continuar usando `ASPNETCORE_ENVIRONMENT=Production`
  e não publicar essas rotas.

## Alternativas consideradas

- Manter Compose em `Production`: rejeitada, pois impede o uso local do Scalar.
- Mapear Scalar também em Production: rejeitada, pois ampliaria a superfície
  exposta em deploys sem uma decisão de publicação específica.
- Alterar apenas o ambiente local em execução manualmente: rejeitada como
  solução permanente, pois depende de ajuste manual a cada clone.

## Consequências e impacto

- **Docker Compose:** o serviço local da API passa a usar `Development`.
- **Infrastructure/HTTP:** sem mudança de código; `Program.cs` já mapeia Scalar
  e OpenAPI exclusivamente quando `IsDevelopment()`.
- **Deploys:** nenhuma configuração Production é alterada por esta decisão.
- **Segurança:** Scalar local fica acessível pela porta publicada 8080; não
  publicar essa configuração local como ambiente de deploy.
- **Stack:** não há pacote, serviço ou camada novos; apenas o perfil do Compose.

## Limites

Esta decisão se aplica ao Compose local. Não aprova Scalar/OpenAPI em Production
nem muda os demais critérios da ADR-010.
