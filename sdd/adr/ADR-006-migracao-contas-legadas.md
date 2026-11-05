# ADR-006 — Migração segura das Contas legadas

**Status:** Aceita em 2026-11-05.

## Contexto

A PoC possui Contas com `Id` e `Nome`, sem Categoria, natureza ou classificador.
A F03 exige vínculo com Categoria, mas não há informação confiável para inferir se
cada registro legado é receita ou despesa. Atribuir uma Categoria automaticamente
reinterpretaria dados do usuário e violaria a preservação histórica.

## Decisão

Adotar migração **expand–migrate–contract**:

1. adicionar `categoria_id`, `classificador` e `ativa`, preservando IDs e nomes;
2. permitir `categoria_id = null` somente para registros legados já existentes;
3. exigir Categoria ativa e `categoria_id` não nulo na criação de toda Conta nova;
4. expor a pendência de classificação nas consultas, sem inventar natureza;
5. impedir que uma Conta sem Categoria receba novos Lançamentos;
6. realizar backfill explícito em fluxo futuro de reclassificação;
7. após não existirem pendências, criar migration contract tornando a FK obrigatória.

Também ficam ratificadas para a F03: unicidade global do nome, IDs `Guid`,
renomeação permitida sem troca de identidade, paginação igual à F01 e os contratos
HTTP descritos na SPEC-F03.

## Alternativas rejeitadas

- Criar Categoria artificial “Legado/Não classificada”: exigiria escolher uma
  natureza falsa e contaminaria consolidações financeiras.
- Excluir ou recriar Contas: perderia identidade e possíveis vínculos históricos.
- Bloquear o deploy quando houver dados: preserva semântica, mas impede evolução e
  não oferece caminho incremental de saneamento.

## Consequências e impacto

- **Domain:** fábrica de novas Contas sempre exige Categoria; materialização legada
  admite vínculo ausente durante a transição.
- **Application:** criação consulta Categoria e aplica natureza/classificador.
- **Infrastructure/EF Core:** FK nullable na fase expand, sem cascade delete; novas
  gravações passam exclusivamente pelo caso de uso restritivo.
- **HTTP:** respostas podem apresentar `categoriaId: null` para legado; POST nunca o aceita.
- **Migração futura:** a fase contract é obrigatória antes de considerar RN-002
  integralmente satisfeita para todo o acervo.

## Risco residual

Até o backfill, Contas legadas continuam consultáveis e renomeáveis, mas não estão
aptas a receber lançamentos nem a participar de consolidações por Categoria.
