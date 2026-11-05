# ADR-005 — Contratos e políticas de Categoria na F01

**Status:** Aceita em 2026-11-05.

## Contexto

A SPEC-F01 dependia da definição da política de nome, renomeação, identificadores,
paginação e contratos HTTP de Categoria (DP-02, DP-03, DP-07, DP-08 e DP-09).

## Decisão

- Categoria usa `Guid`, preservando o padrão da PoC.
- O nome é obrigatório, recebe `Trim`, tem no máximo 100 caracteres e é único
  globalmente após normalização sem distinção de caixa. O Domain publica a regra e
  o RequestValidator a reutiliza; o SQLite mantém índice único como garantia final.
- Categoria nasce ativa e sua natureza (`RECEITA` ou `DESPESA`) não pode ser alterada.
- Renomear preserva ID e natureza e é permitido mesmo com histórico. A F01 não
  mantém auditoria dos nomes anteriores.
- A listagem aceita `natureza`, `ativa`, `pagina` (padrão 1) e `tamanhoPagina`
  (padrão 20, máximo 100), ordenando por nome e ID.
- São aprovadas as rotas `POST /categorias`, `GET /categorias`,
  `GET /categorias/{id}` e `PATCH /categorias/{id}/nome`.
- Falhas esperadas usam `Result`; a Infrastructure as traduz pelo mapper HTTP comum.

## Alternativas consideradas

- IDs inteiros: rejeitados para evitar conversão da convenção atual baseada em Guid.
- Bloquear renomeação com histórico: rejeitado porque nome é rótulo e a natureza e o
  identificador permanecem estáveis.
- Listagem sem limites: rejeitada para manter custo previsível.
- Duplicar a expressão de validação no FluentValidation: rejeitada pela ADR-003.

## Consequências e impacto

- **Domain:** `NaturezaCategoria`, `NomeCategoria` e `Categoria` concentram invariantes.
- **Application:** casos de uso orquestram criação, consulta, listagem e renomeação.
- **Infrastructure/HTTP:** Requests e validators explícitos, contratos em pt-BR e
  paginação aprovada.
- **Infrastructure/EF Core:** tabela `categorias`, nomes de colunas em pt-BR e índice
  único em `nome_normalizado`, criado por migration versionada.
- **STACK.md:** deve registrar a extensão da stack existente à F01.

## Limites

Inativação/reativação pertence à F02. Seed das onze categorias pertence à F09.
