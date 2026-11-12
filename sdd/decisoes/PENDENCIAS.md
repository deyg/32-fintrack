# Pendências para fechamento das SPECs

As pendências abaixo não constituem decisões aprovadas, exceto quando a ação
está explicitamente marcada como **Resolvida**. Usar o código DP em ADRs e
relatórios.

| ID | Tema | Features impactadas | Próxima ação |
|---|---|---|---|
| DP-01 | Unicidade de Conta global ou por Categoria | F03 | Ratificar regra vigente da PoC (global) ou mudar com migração. |
| DP-02 | Nome/limite/unicidade da Categoria | F01, F09 | Definir contrato do catálogo. |
| DP-03 | Renomear Categoria/Conta com lançamentos | F01, F03, F07, F08 | Especificar política histórica. |
| DP-04 | Reclassificar Conta / mover de Categoria com histórico | F04, F07, F08 | Definir bloqueio ou versionamento. |
| DP-05 | Reativação de Categoria/Conta | F02, F04 | Definir condições e mensagens. |
| DP-06 | Escala e arredondamento monetário; data/timezone | F05, F07, F08 | Definir valor decimal, períodos e data. |
| DP-07 | Ids Guid (PoC) ou int (ER original) | Todas | ADR técnico e plano de migração. |
| DP-08 | Paginação, filtros e ordenação | F01, F03, F07 | Definir contrato de consulta. |
| DP-09 | Rotas, contratos e política HTTP das novas operações | Todas | Aprovar OpenAPI por feature. |
| DP-10 | Data futura, motivo e data de cancelamento | F05, F06 | Ratificar regras de realização/cancelamento. |
| DP-11 | Carga única de demonstração de Contas e Lançamentos | F13 | Resolvida em 2026-11-12 por aprovação do usuário; ver `REQUISITOS.md` seção 6.1, SPEC-F13 e ADR-012. |

**Política de execução:** enquanto uma decisão impactar o comportamento de uma feature e estiver pendente, a Skill deve apresentar a alternativa e impedir que a suposição seja promovida a regra permanente. Avançar somente com partes não bloqueadas.
