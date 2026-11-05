# SPEC-F02 — Inativar Categoria

**Status:** Proposta · **Dependência:** F01 + F03 para testes de bloqueio · **Bloqueios:** DP-05, DP-09.  
**Rastreabilidade:** RF-004, RF-005; RN-015, RN-018, RN-023.

## História

Como usuário, quero inativar uma categoria sem apagar contas ou lançamentos, para impedir novas associações e conservar o histórico.

## Contratos propostos

- `PATCH /categorias/{id}/inativacao`: inativa categoria existente.
- `PATCH /categorias/{id}/ativacao`: reativação **condicional** à aprovação de DP-05.
- Respostas: 200 ou 204 no sucesso (definir); 404 para ID inexistente; conflito de estado conforme política aprovada.

## Regras

- Categoria inativa permanece consultável no histórico.
- Categoria inativa não permite criar novas Contas (RN-015).
- Contas existentes e seus lançamentos não são apagados nem reclassificados.
- Definir o comportamento de contas já ativas sob categoria inativa antes de registrar novos lançamentos (proposta de bloqueio preventivo).
- Reativação da categoria não é automática. Depende de DP-05.

## Cenários de aceite

1. Inativar categoria ativa mantém a linha e altera somente o estado.
2. Cadastrar nova Conta numa categoria inativa retorna falha esperada.
3. Consultar Conta/Lançamento vinculado continua funcionando após inativação.
4. Inativar ID inexistente retorna 404.
5. Repetir inativação e reativar obedece à decisão aprovada, sem comportamento implícito.
6. Nenhum registro histórico é excluído por cascata.

## Componentes

`Categoria.Inativar()` no Domain, caso de uso correspondente na Application, endpoint e persistência no Infrastructure. Testes de integração com SQLite e uma categoria com contas vinculadas.
