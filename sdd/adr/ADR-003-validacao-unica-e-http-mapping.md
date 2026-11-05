# ADR-003 — Política única de nome e mapeamento HTTP centralizado

**Status:** Aceita (evolução autorizada na revisão do Harness v1.7).

## Contexto
No v1.6 o RequestValidator e a entidade codificavam separadamente a regra de nome; o endpoint mapeava Result para HTTP com um switch próprio.

## Decisão
- `Domain/ValueObjects/NomeConta` define e normaliza a regra **uma única vez**.
- `Conta.Criar` utiliza `NomeConta.Criar`, protegendo a entidade.
- `CriarContaRequestValidator` (Infrastructure) usa FluentValidation para invocar `NomeConta.Criar` e retornar falhas de campo antes da operação; não duplica a implementação da regra.
- `ResultHttpExtensions` (Infrastructure / Inbound) centraliza o mapeamento de `Result<T>` para `ProblemDetails`, incluindo código estável de erro e status HTTP.
- Testes HTTP com `WebApplicationFactory<Program>` e SQLite temporário verificam o contrato real, sem substituir testes unitários.

## Consequências
- A validação da borda e a validação do Domain são executadas em momentos distintos, mas compartilham **uma definição** da regra.
- A Infrastructure pode referenciar tipos públicos de domínio; o Domain continua sem dependência de FluentValidation ou HTTP.
- A regra de unicidade permanece garantida exclusivamente pelo índice do SQLite.
- A PoC mantém `EnsureCreatedAsync`; migrations ficam para uma evolução específica.
