# Relatório — Harness / FinTrack PoC v1.8

## Implementado
- Migração inicial EF Core versionada (`InitialCreate`) com tabela `Contas` e índice `UNIQUE` de `NomeNormalizado`.
- `Program.cs` não executa mais `EnsureCreatedAsync`; schema deve ser atualizado explicitamente antes do start da API.
- `Microsoft.EntityFrameworkCore.Design` e `dotnet-ef` 10.0.14 registrados.
- `FinTrackApiFactory` aplica migrations a SQLite temporário antes de testes HTTP.
- Testes de migração, idempotência, endpoints 201/400/409 e health adicionados ou preservados.
- ADR-004 e STACK.md atualizados.
- Workflow `.github/workflows/fintrack-ci.yml` pronto para rodar .NET 10 em GitHub Actions após push; **não executado** aqui.

## Limitações de execução
- Ambiente **sem SDK dotnet**, compilador .NET e conectividade ao feed NuGet. Assim, `dotnet restore`, `dotnet build`, `dotnet test`, `dotnet ef ...` e HTTP real **não foram executados**.
- Migration e snapshot foram **escritos manualmente**, sem geração oficial pelo EF Tools; exigem conferência com `dotnet ef migrations has-pending-model-changes` e validação de C# em ambiente com SDK.
- Uma simulação SQL equivalente foi executada via SQLite local (relatório de auditoria separado), mas não valida EF Core, ASP.NET Core ou xUnit.

## Verificação a executar no Codex com .NET 10
```bash
dotnet tool restore
dotnet restore FinTrack.slnx
dotnet build FinTrack.slnx --no-restore
dotnet ef migrations list --project src/FinTrack.Infrastructure --startup-project src/FinTrack.Infrastructure --no-connect
dotnet ef migrations has-pending-model-changes --project src/FinTrack.Infrastructure --startup-project src/FinTrack.Infrastructure
dotnet ef database update --project src/FinTrack.Infrastructure --startup-project src/FinTrack.Infrastructure
dotnet test FinTrack.slnx --no-build
dotnet run --project src/FinTrack.Infrastructure --urls http://localhost:5100
```

Em outro terminal, exercitar HTTP 201/409/400/200, conforme README.

## Estado
**Código preparado; compilação, testes de integração e HTTP real não verificados.** Não atribuir 4,5 estrelas por evidência insuficiente.
