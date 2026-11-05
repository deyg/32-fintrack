# ADR-001 — Propriedade das validações por camada

Status: Substituída por [ADR-002](ADR-002-validacao-antecipada-http.md).

A decisão original evitava duplicar toda validação de negócio na borda. A ADR-002
mantém essa orientação geral, mas permite validação antecipada e deliberada das
invariantes de entrada também no Request HTTP para melhorar o feedback do cliente.
