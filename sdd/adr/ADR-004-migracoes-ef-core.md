# ADR-004 — Migrations explícitas do EF Core

- **Status:** Aceita (evolução autorizada ao solicitar melhoria de prontidão da PoC)
- **Contexto:** `EnsureCreatedAsync` cria o esquema sem histórico de migrações e impede evolução controlada.
- **Decisão:** Adotar EF Core Migrations com código versionado na Infrastructure. A API não altera o esquema no startup. Em desenvolvimento, executar `dotnet ef database update` antes de iniciar a API; testes usam `Database.Migrate()` em SQLite isolado. Produção deve aplicar migrations no pipeline, com revisão de script SQL ou bundle.
- **Alternativa:** `MigrateAsync` no startup; rejeitada para produção por questões operacionais e de concorrência.
- **Consequência:** Desenvolvimento e deploy devem aplicar migrations explicitamente; bancos de PoC criados com `EnsureCreated` exigem transição planejada ou recriação se forem descartáveis.
- **Limites:** Nenhum pacote EF Core, DbContext ou tipo HTTP no Core.
