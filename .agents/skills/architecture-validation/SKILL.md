---
name: architecture-validation
description: Audita uma Solution .NET quanto à arquitetura definida, referências entre projetos, uso da stack, build e testes; gera relatório de conformidade sem modificar o código.
---

# Architecture Validation

## Objetivo
Verificar se a implementação respeita as decisões registradas, com evidências reais.

## Procedimento
1. Ler `STACK.md` (inclusive sua matriz `Tecnologia | Camada/local | Responsabilidade | Restrições`), ADRs, SPECs aplicáveis e arquivos `.sln`/`.slnx` e `.csproj`.
2. Verificar se cada tecnologia e pacote estão na camada/local previsto, se cumprem suas restrições e se a matriz está completa para a stack realmente usada. Inspecionar `using`, `.csproj`, DI e implementações, não apenas nomes de pastas.
3. Verificar a direção `Infrastructure → Core`, ausência de referência inversa, e dependência `Application → Domain` sem referência `Domain → Application`.
4. Verificar que HTTP, persistência e DI não vazaram para Domain; que FluentValidation e seus validators estão exclusivamente na Infrastructure/Inbound; que qualquer repetição de invariantes na validação HTTP é explícita, mínima, documentada em ADR e compatível com o Domain; e que falhas esperadas usam Result, conforme a stack.
5. Quando houver API, conferir que `Request`/`Response` e eventuais `RequestValidator` HTTP estão na Infrastructure, `Command`/`UseCase` estão organizados por feature na Application, e o mapeamento `Request → Command` é explícito.
6. Verificar que validação antecipada HTTP reutiliza regras públicas do Domain sem cópias de expressões e que Result → ProblemDetails é centralizado, sem switch repetido em endpoints. Verificar uso de `ProblemDetails` e exception handling na Infrastructure, se houver Web API.
7. Conferir `CancellationToken` nos fluxos assíncronos de Application e Infrastructure e nos ports de I/O; verificar propagação do endpoint até chamadas externas, sem uso indevido de `CancellationToken.None`. Cancelamento solicitado não deve virar `Result.Failure` de negócio nem HTTP 500; não exigir token em métodos síncronos simples do Domain.
8. Se existir API e projeto de testes, verificar integração HTTP com `WebApplicationFactory` (sucesso, 400, 409 e banco isolado). Executar `dotnet restore`, `dotnet build` e `dotnet test` quando existirem os projetos e o SDK adequado.
9. Se EF Core Migrations estiver definida no STACK.md, verificar ausência de `EnsureCreated` em produção, migrações versionadas, histórico e cenário de instalação de banco vazio; executar `dotnet ef migrations has-pending-model-changes` quando houver SDK. Não confundir teste SQL simulado com teste EF Core.
10. Produzir relatório com: **Conforme**, **Não conforme** ou **Não verificado**, indicação de tecnologia/camada esperada versus encontrada, arquivo/linha para cada achado, impacto e correção recomendada. Classificar mapeamento ausente como **Não verificado**, nunca como **Conforme**.

## Limites
- Não corrigir automaticamente, instalar pacotes ou modificar arquivos.
- Não declarar testes ou build aprovados sem sua execução.
- Ausência de testes deve ser informada como lacuna, não como aprovação.
