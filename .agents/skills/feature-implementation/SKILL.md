---
name: feature-implementation
description: Implementa uma feature .NET por vez a partir da SPEC aprovada, respeitando STACK.md, ADRs, Hexagonal, FluentValidation e Result Pattern. Use para criar ou alterar código de negócio com testes.
---

# Feature Implementation (SDD)

## Objetivo
Entregar uma feature completa, pequena e rastreável, sem alterar a arquitetura por conveniência.

## Procedimento
1. Ler a SPEC informada (preferencialmente em `sdd/specs/`), a **matriz Tecnologia × Camada/local × Responsabilidade × Restrições** do `STACK.md`, ADRs e código existente.
2. Identificar entradas, regras, casos de uso, interfaces, adaptadores, critérios de aceite e testes necessários. Antes de gerar cada arquivo ou adicionar pacote, conferir sua camada e restrições na matriz. Se faltar mapeamento necessário, registrar a lacuna e solicitar decisão via `stack-definition`/ADR, sem adivinhar.
3. Criar cada feature em `Core/Application/UseCases/<Feature>/`, reunindo `Command` e `UseCase`. Não criar `Validator` de Command nem pasta genérica `DTOs` por padrão.
4. Quando houver HTTP, colocar `Request` e eventual `RequestValidator` em `Infrastructure/Adapters/Inbound/Http/<Feature>/` e mapear explicitamente `Request → Command`; não expor `Command` diretamente como contrato HTTP. FluentValidation pode validar antecipadamente o Request HTTP; quando a regra pertencer ao Domain, REUTILIZAR sua política/fábrica pública em vez de reescrevê-la na borda. A entidade ainda valida sua invariante, e a decisão é registrada em ADR.
5. Implementar entidades, invariantes e ports no Domain; casos de uso e pré-condições específicas da operação na Application; validação de transporte (se existir), API, persistência e integrações na Infrastructure, apenas quando a SPEC exigir.
6. Preservar invariantes exclusivamente no Domain com `Result`/`Result<T>` em falhas esperadas; na Application apenas orquestrar e propagar esses resultados. Aplicar FluentValidation somente a Requests HTTP na Infrastructure quando aprovado, podendo repetir deliberadamente invariantes de entrada documentadas em ADR; não delegar proteção do Domain ao validator. Mapear Result/erros para HTTP via componente compartilhado de Infrastructure (ProblemDetails), evitando `switch` duplicado por endpoint.
7. Utilizar DI nativa; não introduzir MediatR, frameworks ou projetos sem decisão. Em persistência, garantir no banco as restrições de integridade e concorrência previstas na SPEC.
8. Incluir `CancellationToken cancellationToken` em métodos assíncronos de UseCases, interfaces/ports de I/O e adapters. Obter o token recebido do ASP.NET Core no endpoint, propagá-lo sem substituição para eventual `ValidateAsync` de Request na Infrastructure, EF Core, `HttpClient` e demais operações de I/O que aceitem cancelamento. Não usar `CancellationToken.None` ou criar novos tokens ignorando o original. Não exigir token em operações síncronas do Domain.
9. Implementar testes da feature no projeto de testes existente, incluindo integração HTTP com `WebApplicationFactory<Program>` e banco isolado quando existir Web API; caso contrário, registrar a lacuna sem criar terceiro projeto silenciosamente.
10. Quando houver mudança de esquema EF Core, criar migration versionada e testar aplicação em banco novo, sem migração automática no startup de produção.
11. Executar verificações disponíveis (`dotnet build`, `dotnet test`) e conferir critérios de aceite.
12. Relatar arquivos alterados, critérios atendidos, testes executados e pendências.

## Limites
- Se a SPEC estiver ausente ou ambígua, não inventar regras: solicitar a decisão estritamente necessária ou limitar-se ao escopo seguro.
- Não introduzir dependências ou bibliotecas em camadas proibidas pela matriz do `STACK.md` (ex.: EF Core no Core, ASP.NET Core no Domain).
- Não reestruturar toda a Solution durante uma feature.
- Não criar commits sem autorização.

## Cancelamento
- Cancelamento cooperativo não garante rollback de operações já concluídas.
- `OperationCanceledException` por cancelamento solicitado não é falha de negócio esperada; não convertê-la em `Result.Failure` nem tratá-la como HTTP 500.
