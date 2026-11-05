# SPEC-F06 — Cancelar Lançamento

**Status:** Aprovada em 2026-11-05 · **Dependência:** F05 · **Decisão:** ADR-008.  
**Rastreabilidade:** RF-011; RN-013, RN-014, RN-018.

## História

Como usuário, quero cancelar um registro financeiro incorreto preservando sua rastreabilidade, sem excluí-lo fisicamente.

## Contrato proposto

`POST /lancamentos/{id}/cancelamento` com corpo opcional `{ "motivo": "Lançamento em duplicidade" }`. Resposta de sucesso 200/204 (a confirmar). Não implementar endpoint de reativação.

## Regras

- `REALIZADO → CANCELADO` é a única transição para um lançamento realizado.
- Lançamento cancelado não pode voltar para REALIZADO (regra já definida).
- Não excluir fisicamente o registro nem alterar valor/Conta/data para simular cancelamento.
- Lançamentos cancelados não entram em totais financeiros (proposta para ratificação).
- Guardar data/motivo do cancelamento é proposta; política do motivo e repetição de cancelamento dependem de DP-10.

## Cenários de aceite

1. Cancelar lançamento realizado altera sua situação sem apagar a linha.
2. Histórico preserva valor, data e Conta originais.
3. Lançamento cancelado é ignorado pelos consolidados.
4. Segundo cancelamento recebe resposta previsível segundo política aprovada.
5. Tentativa de reativação é impossível pela API e pelo Domain.
6. ID inexistente retorna 404; concorrência de cancelamento não gera estados inválidos.

## Componentes

Domain `Lancamento.Cancelar`, Application caso de uso específico, Infrastructure endpoint/EF Core, testes de transições e integração.
