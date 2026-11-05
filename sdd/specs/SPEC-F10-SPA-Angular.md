# SPEC-F10 — Operar o FinTrack pela SPA Angular

**Status:** Aprovada pelo pedido de implementação em 2026-11-05.

## Objetivo

Oferecer uma interface Angular de página única, responsiva e em tema escuro para
operar as features já disponíveis na API do FinTrack.

## Escopo

- Exibir visão geral financeira por período.
- Criar, listar, renomear, ativar e inativar Categorias.
- Criar, listar, renomear, ativar, inativar e alterar o classificador de Contas.
- Registrar, filtrar e cancelar Lançamentos.
- Consultar consolidados geral, por Conta, Categoria e Classificador.
- Exibir estados de carregamento, vazio, sucesso e erro retornado pela API.

## Restrições

- Aplicação Angular standalone em `src/FinTrack.Web`.
- Página única, tema black e layout responsivo.
- Contratos em português brasileiro.
- Nenhuma regra financeira é redefinida no cliente; a API permanece como fonte
  de verdade.
- Sem autenticação, gráficos externos ou alteração de contratos HTTP neste
  incremento.

## Critérios de aceite

1. O build de produção do Angular conclui sem erros.
2. Os testes automatizados do frontend concluem sem falhas.
3. Todas as operações da API do MVP ficam acessíveis pela interface.
4. Erros `400`, `404`, `409`, `422` e `500` podem ser apresentados ao usuário.
5. A interface funciona em larguras de desktop e dispositivo móvel.

