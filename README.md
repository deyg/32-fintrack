# FinTrack — MVP do Harness v1.8

MVP financeiro em .NET 10, ASP.NET Core Minimal API, EF Core + SQLite,
FluentValidation e Result Pattern. Implementa Categorias, Contas, Lançamentos,
cancelamento, consultas, consolidação e seed idempotente, preservando a transição
das Contas legadas descrita na ADR-006.

## Executar (requer .NET SDK 10 e NuGet)

```bash
dotnet tool restore
dotnet restore FinTrack.slnx
dotnet build FinTrack.slnx --no-restore
dotnet ef migrations list --project src/FinTrack.Infrastructure --startup-project src/FinTrack.Infrastructure --no-connect
dotnet ef migrations has-pending-model-changes --project src/FinTrack.Infrastructure --startup-project src/FinTrack.Infrastructure
dotnet ef database update --project src/FinTrack.Infrastructure --startup-project src/FinTrack.Infrastructure
dotnet test FinTrack.slnx --no-build
dotnet run --project src/FinTrack.Infrastructure
```

Para carregar explicitamente as onze categorias iniciais após aplicar migrations:

```bash
dotnet run --project src/FinTrack.Infrastructure -- --inicializar-categorias
```

Rotas principais: `/categorias`, `/contas`, `/lancamentos`, `/consolidados` e `/health`.
As evidências finais estão em `sdd/progresso/AUDITORIA-MVP.md`.

O banco SQLite `fintrack.db` é criado/atualizado explicitamente com `dotnet ef database update`, sem `EnsureCreatedAsync` no startup. A migração inicial está em `Persistence/Migrations/`, e o projeto de testes aplica migrations em banco isolado. As migrations foram escritas manualmente porque o SDK .NET não está disponível neste ambiente; valide a consistência do modelo com `dotnet ef migrations has-pending-model-changes` antes de utilizá-las. Para produção, revise scripts SQL ou use migration bundles no pipeline de deploy; não migre automaticamente na inicialização da API.

## Testar endpoint

```bash
curl -i -X POST http://localhost:5000/contas -H 'Content-Type: application/json' -d '{"nome":"Conta Corrente"}'
```

A URL real deve ser a publicada por `dotnet run`; ajuste para a porta indicada no console. Enviar o nome uma segunda vez (inclusive em minúsculas) deve produzir HTTP 409. Nome vazio ou com mais de 100 caracteres após `Trim()` deve produzir HTTP 400 com erro em `nome`.

## Codex

```text
$feature-orchestrator
Implemente ou revise a SPEC-001. Leia STACK.md e os ADRs. Valide com build e testes.
```

Os oito arquivos `SKILL.md` do Harness v1.8 estão em `.agents/skills/`. A política atualizada está em `sdd/adr/ADR-003-validacao-unica-e-http-mapping.md`. `AGENTS.md` orienta o Codex neste repositório.

## Verificação

Veja `REPORT.md`. Os comandos de compilação e testes dependem de SDK disponível e de acesso aos pacotes NuGet.

## Evolução v1.8

- Regra única de nome no domínio: `Domain/ValueObjects/NomeConta.cs`.
- FluentValidation no Request HTTP reutiliza a mesma regra via `NomeConta.Criar`.
- `ResultHttpExtensions` elimina os `switch` de mapeamento HTTP dos endpoints.
- Testes de integração HTTP reais via `WebApplicationFactory`, com banco SQLite temporário isolado.
- `EnsureCreatedAsync` é intencional para a PoC; não representa prontidão para produção.

## Smoke test HTTP (após subir a API)

Em outro terminal, com a URL informada por `dotnet run` (exemplo com porta 5100):

```bash
curl -i -X POST http://localhost:5100/contas -H "Content-Type: application/json" -d '{"nome":"Conta Corrente"}'
curl -i -X POST http://localhost:5100/contas -H "Content-Type: application/json" -d '{"nome":"conta corrente"}'
curl -i -X POST http://localhost:5100/contas -H "Content-Type: application/json" -d '{"nome":""}'
curl -i http://localhost:5100/health
```

Esperado: 201, 409, 400 e 200. Para fixar a porta: `dotnet run --project src/FinTrack.Infrastructure --urls http://localhost:5100`.

**Atenção a bancos de versões anteriores:** SQLite criado por `EnsureCreated` não terá `__EFMigrationsHistory`; não aplique a migração inicial diretamente a um banco com tabelas preexistentes. Para a PoC, recrie apenas o banco descartável ou prepare uma migração de adoção após backup/inspeção dos dados.

## CI

A workflow `.github/workflows/fintrack-ci.yml` está preparada para executar build, verificação de migrations e testes xUnit/HTTP no GitHub após o push. Nenhuma execução no GitHub foi realizada nesta entrega.

## Evolução funcional SDD v0.1

Consulte [PLAYBOOK.md](PLAYBOOK.md), [REQUISITOS.md](sdd/requisitos/REQUISITOS.md), [DOMINIO.md](sdd/dominio/DOMINIO.md) e as nove SPECs F01–F09 em `sdd/specs/`. Estes documentos registram a **evolução pretendida**, ainda não implementada, da PoC para Categoria → Conta → Lançamento. A implementação de código existente permanece no estágio v1.8; não se deve confundir este pacote documental com features executadas.
