# Auditoria arquitetural — F01 Gerenciar Categorias

**Data:** 2026-11-05  
**Resultado:** Conforme

| Verificação | Estado | Evidência |
|---|---|---|
| Direção de dependências | Conforme | Core não possui ProjectReference; Infrastructure referencia somente Core |
| Tecnologias por camada | Conforme | Core sem pacotes; EF Core e FluentValidation somente na Infrastructure |
| Domain/Application | Conforme | invariantes em `Categoria`/`NomeCategoria`; Commands e UseCases na Application |
| HTTP | Conforme | Requests/validators/endpoints na Infrastructure; Request convertido explicitamente em Command |
| Regra compartilhada | Conforme | validators chamam `NomeCategoria.Criar`; nenhuma expressão de limite duplicada |
| Result → HTTP | Conforme | endpoints reutilizam `ResultHttpExtensions`; switch permanece apenas no mapper comum |
| CancellationToken | Conforme | endpoint → validator/use case → port → EF Core |
| Persistência | Conforme | tabela/colunas de Categoria em pt-BR, índice único e migration versionada |
| Startup | Conforme | ausência de `EnsureCreated`/`Migrate` no código de produção |
| Testes HTTP | Conforme | `WebApplicationFactory`, SQLite isolado, cenários 201/200/400/404/409 |

## Evidências executadas

- `dotnet restore FinTrack.slnx`: concluído, projetos atualizados.
- `dotnet build FinTrack.slnx --no-restore`: aprovado, 0 avisos e 0 erros.
- `dotnet test FinTrack.slnx --no-build`: 41 aprovados, 0 falhas, 0 ignorados.
- `dotnet ef migrations has-pending-model-changes ...`: nenhuma mudança pendente.
- Migrations listadas: `20261105000100_InitialCreate` e `20261105120623_AddCategorias`.
- `git status --short`: limpo antes deste relatório.

## Observação fora do escopo

A tabela legada `Contas` ainda usa nomenclatura anterior; sua evolução pertence à
F03 e exige planejamento de migração dos dados existentes. Isso não afeta a
conformidade da F01 nem deve ser corrigido silenciosamente nesta feature.
