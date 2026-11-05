# Instruções para Codex

- Consultar `sdd/specs/`, `STACK.md` e `sdd/adr/` antes de implementar features.
- Usar `$feature-orchestrator` para coordenar as Skills de cada feature.
- Respeitar dependência `Infrastructure → Core`, nunca inversa.
- Manter Request e RequestValidator na Infrastructure; Command e UseCase na Application. As invariantes continuam no Domain; validação HTTP antecipada deliberada pode repetir condições (ADR-002), mas não substitui o Domain.
- Usar `CancellationToken` em fluxos assíncronos até I/O.
- Executar restore, build, tests e relatar evidências; não declarar aprovação sem executar.
- Para implementação de código do FinTrack, há autorização expressa para **um commit por artefato de código produzido ou modificado**, após validação; registrar SHA em `sdd/progresso/REGISTRO-COMMITS.md`. Não fazer push sem autorização.

- Para validações antecipadas, reutilizar a regra publicada pelo Domain (não duplicar expressões).
- Traduzir resultados HTTP pelo mapper comum da Infrastructure, não por `switch` em cada endpoint.
- Novas features HTTP devem contemplar testes de integração com `WebApplicationFactory` quando projeto de testes existir.

- Usar EF Core Migrations versionadas para criar/evoluir o SQLite. Nunca usar `EnsureCreated` em produção; migrations são aplicadas no deploy, não no startup.

- Ler `PLAYBOOK.md` e `sdd/requisitos/REQUISITOS.md` antes de implementar qualquer SPEC-Fxx.
- Os documentos SDD v0.1 são **propostas**; não transformar DP pendente em regra aprovada por suposição.
- Nomes de tabelas, colunas, entidades, propriedades e métodos de negócio em pt-BR, sem acentos em identificadores.
