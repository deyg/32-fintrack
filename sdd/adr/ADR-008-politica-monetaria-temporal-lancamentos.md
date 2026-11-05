# ADR-008 — Política monetária, temporal e histórica de Lançamentos

**Status:** Aceita em 2026-11-05.

## Decisão

- Valores usam `decimal(18,2)`, são estritamente positivos e entradas com mais de
  duas casas decimais são rejeitadas, sem arredondamento silencioso.
- `DataRealizacao` usa `DateOnly`, representa a data civil informada e não pode ser
  futura em relação ao relógio fornecido ao caso de uso.
- Descrição e motivo são opcionais, recebem `Trim` e têm no máximo 200 caracteres.
- Lançamento nasce `REALIZADO`; cancelamento é lógico, irreversível e idempotente,
  registrando instante UTC e motivo opcional.
- Consultas incluem realizados e cancelados por padrão, com filtro opcional;
  intervalos são inclusivos, página 1, tamanho 20, máximo 100, ordenação por data
  decrescente e ID.
- Consolidados consideram somente realizados, usam a classificação vigente da
  Conta e Categoria, e retornam valores com duas casas decimais.
- Alterar classificador de Conta é bloqueado quando existir qualquer Lançamento.

## Consequências

Domain protege valor, escala, data e transição; Application recebe a data atual;
EF Core persiste sem duplicar natureza/classificador; HTTP usa contratos em pt-BR.
