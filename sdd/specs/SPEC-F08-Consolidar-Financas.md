# SPEC-F08 — Consolidar Finanças

**Status:** Aprovada em 2026-11-05 · **Dependências:** F05/F06/F07 · **Decisão:** ADR-008.  
**Rastreabilidade:** RF-013, RF-014; RN-007, RN-014, RN-017, RN-019, RN-020, RN-022.

## História

Como usuário, quero visualizar receitas, despesas, resultado e totais agrupados para entender minha situação financeira em um período.

## Contratos propostos

- `GET /consolidados?dataInicial=...&dataFinal=...`
- `GET /consolidados/por-conta?dataInicial=...&dataFinal=...`
- `GET /consolidados/por-categoria?dataInicial=...&dataFinal=...`
- `GET /consolidados/por-classificador?dataInicial=...&dataFinal=...`

Formato de saída sugerido para totais gerais:

```json
{"receitas": 8625.50, "despesas": 4079.90, "resultado": 4545.60,
 "dataInicial":"2026-10-01", "dataFinal":"2026-10-31"}
```

*Valores acima são ilustrativos; não são uma expectativa de teste e exigem conciliação com fixtures reais.*

## Regras

- Somar `valor` de Lançamentos realizados não cancelados no período.
- Receita ou despesa deriva da natureza da Categoria vinculada à Conta.
- Classificador pertence à Conta e existe apenas para despesas.
- Resultado = Receitas − Despesas. Sem cálculo de saldo bancário, pendências ou previsão.
- Não contar lançamentos cancelados, inativos não alteram histórico por si só.
- Preservar interpretação histórica, sem alteração retroativa indevida de categoria/classificador.
- Precisão monetária e timezone/período dependem DP-06.

## Cenários de aceite

1. Com receita de 5000 e despesas de 1500, resultado = 3500.
2. Lançamento cancelado é excluído de todos os agregados.
3. Totais por categoria e por conta reconciliam com total por natureza.
4. Despesas por OF/OV/NOF/NOV reconciliam com total de despesas.
5. Com zero lançamentos, retornar zeros, sem divisão por zero ou erro.
6. Intervalo invertido (`dataInicial > dataFinal`) resulta em erro de entrada.
7. Datas e arredondamento obedecem à política aprovada.

## Componentes

Application casos de uso e contratos de saída sem HTTP, Infrastructure consultas de agregação SQL/EF Core e endpoints; testes de reconciliação e consistência de classificação.
