# STACK — FinTrack PoC

Arquitetura: Hexagonal; dois projetos de produção (`Core`, `Infrastructure`) e um de testes.

| Tecnologia | Camada/local | Responsabilidade | Restrições |
|---|---|---|---|
| .NET 10 / C# | Solution | Plataforma | `net10.0`; requer SDK .NET 10 |
| ASP.NET Core Minimal API | Infrastructure/Adapters/Inbound/Http | `POST /contas` | HTTP fora do Core |
| FluentValidation 12.2.0 | Infrastructure/Adapters/Inbound/Http | Validação antecipada de `CriarContaRequest`, antes do UseCase | Sem dependência no Core; regra de nome delegada a Domain/ValueObjects/NomeConta (ADR-003) |
| Política de nome (NomeConta) | Core/Domain/ValueObjects | Fonte única de validação e normalização | Domain independente de FluentValidation |
| Result Pattern simples | Core/Domain | `Success`, `Failure`, `Match` | Sem HTTP; sem `Bind`/`Map` |
| EF Core SQLite 10.0.14 | Infrastructure/Persistence | Persistência e índice único | EF Core fora do Core |
| SQLite | Infrastructure/Persistence | Banco local | Restrição `UNIQUE` em NomeNormalizado |
| EF Core Migrations + dotnet-ef 10.0.14 | Infrastructure/Persistence/Migrations + Deploy | Evoluir esquema de forma versionada | Sem `EnsureCreated`; aplicar antes de subir a API; revisar migrações |
| Native DI | Infrastructure | Wiring | Sem MediatR |
| ProblemDetails + ResultHttpExtensions | Infrastructure/Inbound | Centralizar tradução Result → HTTP 400/404/409/422; 500 inesperado | Sem HTTP no Core; erros por campo no RequestValidator |
| CancellationToken | Application e Infrastructure | Cancelamento cooperativo | Repassar até I/O; Domain síncrono independente |
| xUnit 2.9.3 + WebApplicationFactory 10.0.14 | Tests | Unitários e integração HTTP + SQLite temporário | Terceiro projeto só de testes; banco isolado por teste |
| Git | DevOps | Versionamento | Não criar commits sem autorização |

Os pacotes da Infrastructure incluem FluentValidation 12.2.0. O `RequestValidator` usa `ValidateAsync` explicitamente e gera `ValidationProblem` (HTTP 400). O Domain conserva as invariantes e o SQLite protege a unicidade. A criação/evolução do banco é feita via migrations versionadas (`dotnet ef database update` no ambiente de desenvolvimento). A API não executa migrations no startup. As migrations desta revisão foram escritas manualmente e exigem verificação com SDK .NET 10 (`has-pending-model-changes`), além de testes de integração executados antes de produção.

ADR-003 centraliza regras de nome em `NomeConta` e mapeamento HTTP em `ResultHttpExtensions`. ADR-004 documenta as migrations e o fluxo de aplicação no deploy.
