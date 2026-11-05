# Auditoria arquitetural final — FinTrack MVP

**Data:** 2026-11-05  
**Escopo:** F01–F09  
**Resultado:** Conforme, com evidências executadas em 2026-11-05.

## Conformidade inspecionada

- Core sem EF Core, ASP.NET Core ou FluentValidation.
- Dependência de projetos somente `Infrastructure → Core`.
- Invariantes em Domain; casos de uso em Application; HTTP, DI e EF na Infrastructure.
- Requests/validators HTTP separados de Commands e UseCases.
- Result traduzido pelo mapper comum; ProblemDetails e exception handler centralizados.
- CancellationToken propagado dos endpoints aos casos de uso e I/O.
- Migrations versionadas; API não migra banco no startup.
- Testes HTTP usam WebApplicationFactory e SQLite isolado.
- Transição legada preserva IDs e não inventa Categoria/natureza.

## Observação operacional

`categoria_id` permanece nullable no esquema somente para suportar Contas legadas da
ADR-006. Novas Contas exigem Categoria. O endpoint de classificação permite o
backfill; a migration contract deve ser aplicada no ambiente somente depois de
confirmar zero Contas pendentes.

## Evidências finais executadas

- `dotnet restore FinTrack.slnx`: projetos atualizados.
- `dotnet build FinTrack.slnx --no-restore`: 0 avisos e 0 erros.
- `dotnet test FinTrack.slnx --no-build`: 45 aprovados, 0 falhas, 0 ignorados.
- `dotnet ef migrations has-pending-model-changes`: nenhuma mudança pendente.
- Migrations: InitialCreate, AddCategorias, ExpandContasComCategoria e AddLancamentos.
- Busca estática: nenhum EF Core, ASP.NET Core, FluentValidation ou Infrastructure no Core.
- Busca de startup: nenhum `EnsureCreated`, `Migrate` ou `MigrateAsync` em produção.
- Smoke test com processo real: `GET http://127.0.0.1:5100/health` retornou
  HTTP 200 e `{"status":"ok"}` sobre banco SQLite migrado do zero.
