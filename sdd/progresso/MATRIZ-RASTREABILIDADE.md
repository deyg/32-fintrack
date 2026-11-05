# Matriz de rastreabilidade — FinTrack SDD v0.1

| Feature | Requisitos funcionais | Regras centrais | SPEC | Implementação | Testes executados | Commits |
|---|---|---|---|---|---|---|
| F01 | RF-001–RF-003 | RN-001, RN-003, RN-010, RN-021 | `SPEC-F01-Categorias.md` + ADR-005 | Domain, Application, EF Core e HTTP implementados | Unitários, persistência, migrations e HTTP executados | FT-F01-001–028 em `REGISTRO-COMMITS.md` |
| F02 | RF-004–RF-005 | RN-015, RN-023 | `SPEC-F02-Inativar-Categoria.md` + ADR-007 | Implementada | HTTP/integrada | FT-F02-001–006 |
| F03 | RF-006–RF-007 | RN-002, RN-004–RN-010 | `SPEC-F03-Contas.md` + ADR-006 | Implementada com transição legada | Domain/Application/EF/HTTP/migration | FT-F03-001–041 |
| F04 | RF-008–RF-009 | RN-016, RN-017 | `SPEC-F04-Situacao-Classificacao-Conta.md` + ADR-007/008 | Implementada | Estados e bloqueio histórico | FT-F04-001–010 |
| F05 | RF-010 | RN-007, RN-011, RN-020 | `SPEC-F05-Registrar-Lancamento.md` + ADR-008 | Implementada | EF/HTTP/integrada | FT-F05-001–016 |
| F06 | RF-011 | RN-013, RN-014 | `SPEC-F06-Cancelar-Lancamento.md` + ADR-008 | Implementada | Cancelamento idempotente integrado | FT-F06-001 + testes F05-016 |
| F07 | RF-012 | RN-007, RN-017 | `SPEC-F07-Consultar-Lancamentos.md` + ADR-008 | Implementada | Filtros/paginação integrados | FT-F07-001 + testes F08-003 |
| F08 | RF-013–RF-014 | RN-014, RN-019 | `SPEC-F08-Consolidar-Financas.md` + ADR-008 | Implementada | Totais e agrupamentos integrados | FT-F08-001–003 |
| F09 | RF-015 | RN-021, RN-024 | `SPEC-F09-Categorias-Iniciais.md` + ADR-005 | Seed explícita e transacional implementada | 2 testes específicos; suíte final 45/45 | FT-F09-001–003 |

Atualizar a matriz **após cada commit de código**, preservando rastreabilidade requisito → regra → SPEC → arquivo → teste → SHA.
