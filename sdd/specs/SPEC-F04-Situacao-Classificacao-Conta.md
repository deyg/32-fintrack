# SPEC-F04 — Situação e Classificação da Conta

**Status:** Aprovada em 2026-11-05 · **Dependências:** F03; histórico F05 para validar bloqueios · **Decisão:** ADR-007; alterações com histórico são bloqueadas.  
**Rastreabilidade:** RF-008, RF-009; RN-004, RN-005, RN-016, RN-017, RN-023.

## História

Como usuário, quero inativar, reativar (conforme política) e administrar o classificador de Contas de despesa sem alterar o significado do meu histórico.

## Contratos propostos

- `PATCH /contas/{id}/inativacao`
- `PATCH /contas/{id}/ativacao` (sujeito a DP-05)
- `PATCH /contas/{id}/classificador` com `{ "classificador": "OV" }` (sujeito a DP-04)

## Regras

- Conta inativa continua consultável e não recebe novos lançamentos (RN-016 proposto).
- Classificador só existe para Conta cuja Categoria tem natureza DESPESA.
- Se houver histórico, alterar classificador pode mudar retrospectivamente consolidações. **Até decisão explícita, bloquear a alteração quando houver lançamentos, mesmo cancelados**; não tratar o bloqueio provisório como regra aceita.
- Não permitir qualquer migração de classificação para Contas de RECEITA.
- Reativação condicionada às regras da Categoria e DP-05.

## Cenários de aceite

1. Inativar Conta não exclui nenhum Lançamento.
2. Novo Lançamento em Conta inativa é recusado.
3. Tentar definir OF em Conta de Receita é recusado.
4. Alterar classificador sem lançamentos segue política aprovada.
5. Alterar classificador com histórico preserva as consolidações (bloqueado até DP-04).
6. Conta inexistente retorna 404; concorrência de alteração respeita o estado.

## Componentes

Domain `Conta.Inativar`, `Conta.Reativar`, `Conta.DefinirClassificador`; Application casos de uso específicos; Infrastructure endpoints e persistência; testes de histórico.
