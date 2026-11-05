# SPEC-F07 — Consultar Lançamentos

**Status:** Proposta · **Dependências:** F05/F06 · **Bloqueios:** DP-03, DP-04, DP-06, DP-08, DP-09.  
**Rastreabilidade:** RF-012; RN-007, RN-013, RN-017.

## História

Como usuário, quero consultar meus lançamentos por período e filtros financeiros para acompanhar o histórico e compreender sua classificação.

## Contrato proposto

`GET /lancamentos?dataInicial=2026-10-01&dataFinal=2026-10-31&categoriaId=...&contaId=...&classificador=OF&situacao=REALIZADO`

Parâmetros podem ser combinados e devem ser opcionais quando o contrato permitir; paginação e limites dependem de DP-08. Resposta: `id, contaId, valor, dataRealizacao, descricao, situacao` e, para visualização, nomes/natureza/classificador derivados da associação vigente conforme política histórica.

## Regras

- Filtrar por `dataRealizacao` com limites inclusivos de data, quando aprovados.
- Permitir filtros de Conta, Categoria, Classificador e Situação.
- Para receitas, `classificador` não existe; não inventar um valor.
- A consulta ao histórico deve permitir visualizar lançamentos CANCELADOS; comportamento padrão (incluir/excluir) fica pendente da política de consulta.
- Paginação/ordenação precisa ser determinística em registros de mesma data.
- Renomeações/reclassificações que mudem interpretação histórica dependem de DP-03/DP-04.

## Cenários de aceite

1. Filtrar por um mês retorna lançamentos na faixa inclusiva aprovada.
2. Filtrar por Conta e Categoria retorna somente vínculos correspondentes.
3. Filtro OF não retorna Contas OV/NOF/NOV nem receitas.
4. Histórico de cancelados é recuperável pelo filtro de situação.
5. Filtros inválidos retornam 400 com descrição clara.
6. Ordenação e paginação não repetem nem perdem itens entre páginas em conjunto estável.

## Componentes

Application caso de uso de consulta via port de leitura; Infrastructure consulta EF Core (`AsNoTracking`) e endpoint. Manter tipos HTTP fora do Core.
