---
name: core-foundation
description: Implementa apenas abstrações genéricas e mínimas do Core .NET, especialmente Result, Result<T>, Error e ErrorType. Use após criar a Solution, antes das features, sem adicionar regras de negócio.
---

# Core Foundation

## Objetivo
Preparar o núcleo sem criar entidades ou casos de uso fictícios.

## Procedimento
1. Inspecionar a Solution, `STACK.md` (inclusive a matriz de tecnologias por camada) e ADRs; localizar o projeto Core. Não incorporar tecnologias atribuídas à Infrastructure.
2. Criar, apenas se não existirem, `Domain/Errors/Error.cs`, `ErrorType.cs` e `Domain/Results/Result.cs`, `ResultOfT.cs` (ou seguir convenções já adotadas).
3. Implementar `Result` e `Result<T>` com `Success`, `Failure` e `Match`; impedir estados inválidos; manter `Error` tipado e independente de HTTP.
4. Não implementar `Map`, `Bind`, `BindAsync` ou bibliotecas externas de Result, salvo decisão posterior.
5. Executar `dotnet build`; quando houver projeto de testes, testar sucesso, falha, Match e estados inválidos.
6. Reportar alterações e evidências de validação.

## Regras
- Domain não depende de Application, Infrastructure ou ASP.NET Core.
- Erros de negócio esperados são resultados, não exceptions para controle de fluxo.
- Exceptions continuam possíveis para erros de programação e situações realmente excepcionais.
- Não adicionar FluentValidation em nenhuma parte do Core (Domain ou Application). Invariantes permanecem no Domain; não implementar funcionalidades de negócio.
