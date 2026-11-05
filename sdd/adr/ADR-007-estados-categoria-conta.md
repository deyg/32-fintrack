# ADR-007 — Estados e reativação de Categoria e Conta

**Status:** Aceita em 2026-11-05.

## Decisão

- Categoria e Conta podem ser inativadas e reativadas sem exclusão ou alteração histórica.
- Operações são idempotentes: repetir o estado desejado retorna sucesso com o recurso atual.
- Categoria inativa bloqueia novas Contas e novos Lançamentos em suas Contas.
- Conta inativa bloqueia novos Lançamentos.
- Reativar uma Conta exige que sua Categoria esteja ativa e classificada.
- Sucesso retorna 200 com o recurso atualizado; inexistência retorna 404.

## Consequências

As invariantes de estado ficam no Domain; Application consulta dependências; HTTP usa
o mapper comum. Nenhuma operação executa exclusão física ou cascata.
