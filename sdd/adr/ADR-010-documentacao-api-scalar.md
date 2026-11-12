# ADR-010 — Referência interativa da API com Scalar

**Status:** Aceita pelo pedido de implementação em 2026-11-12.

## Contexto

A API Minimal do FinTrack não publicava documento OpenAPI nem uma interface para
consultar seus contratos e executar chamadas de desenvolvimento. É necessário
oferecer uma referência interativa sem introduzir dependência no Core ou alterar
as regras de negócio.

## Decisão

- Usar `Microsoft.AspNetCore.OpenApi` para gerar o documento OpenAPI `v1`.
- Usar `Scalar.AspNetCore` no adaptador HTTP da Infrastructure para publicar a
  referência interativa em `/scalar`.
- Expor tanto `/openapi/v1.json` quanto `/scalar` apenas no ambiente
  `Development`.
- Não configurar chaves, tokens, credenciais ou servidores remotos no Scalar.

## Alternativas consideradas

- Swagger UI: não adotada; Scalar entrega a referência e o cliente de testes
  solicitados com uma interface mais atual.
- Documento OpenAPI sem interface: rejeitada, pois não permite explorar e testar
  a API diretamente no navegador.
- Disponibilizar a interface em produção: rejeitada neste incremento para não
  expor uma superfície de exploração operacional sem política de publicação.

## Consequências e impacto

- `Core` permanece sem referências a OpenAPI ou Scalar.
- A dependência e o mapeamento das rotas ficam em `FinTrack.Infrastructure`.
- A documentação permanece alinhada aos endpoints Minimal API publicados.
- Testes de integração devem cobrir a disponibilidade das duas rotas em
  desenvolvimento.
