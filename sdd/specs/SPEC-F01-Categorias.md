# SPEC-F01 — Gerenciar Categorias

**Status:** Aprovada em 2026-11-05 · **Dependências:** base da Solution, modelos do Domain · **Decisões:** ADR-005.  
**Rastreabilidade:** RF-001, RF-002, RF-003; RN-001, RN-003, RN-017, RN-021.

## História

Como usuário, quero cadastrar, consultar, listar e renomear categorias de receita ou despesa para organizar as rubricas de minhas finanças.

## Contratos HTTP aprovados

| Método | Rota | Operação | Resposta de sucesso |
|---|---|---|---|
| POST | `/categorias` | Cadastrar | 201 + `{id,nome,natureza,ativa}` |
| GET | `/categorias` | Listar por natureza/ativa, página 1, tamanho 20 (máximo 100) | 200 + página |
| GET | `/categorias/{id}` | Consultar | 200 + categoria |
| PATCH | `/categorias/{id}/nome` | Renomear | 200 + categoria atualizada |

`POST /categorias` exemplo: `{"nome":"Moradia","natureza":"DESPESA"}`. Contratos JSON em português, nomes de classes e métodos em português sem acentos.

## Regras

- Nome e natureza são obrigatórios. Natureza somente RECEITA ou DESPESA (RN-001).
- Nome obrigatório, `Trim`, máximo de 100 caracteres e unicidade global sem distinção de caixa.
- Categoria criada inicia ativa.
- Renomear é permitido com histórico, preserva ID e natureza e não audita nomes anteriores na F01.
- Identificadores usam `Guid`.
- Listagem ordena por nome e ID e aceita filtros opcionais `natureza` e `ativa`.
- Não apagar categoria com contas vinculadas em cascata.

## Cenários de aceite

1. Cadastrar Categoria RECEITA/ DESPESA válida retorna 201 e é consultável.
2. Natureza desconhecida retorna erro de validação 400.
3. GET com ID inexistente retorna 404.
4. Listagem mostra categorias ativas e inativas conforme filtro aprovado.
5. Renomeação com e sem histórico obedece à política aprovada, sem alterar os identificadores.
6. Conflito de nome retorna 409 quando definição de unicidade for ratificada.
7. Testes de Domain, caso de uso e HTTP devem validar mapeamento `Request → Command`, Result e ProblemDetails.

## Implementação arquitetural

- Domain: `Categoria` e suas invariantes.
- Application: casos de uso `CriarCategoria`, `ConsultarCategoria`, `ListarCategorias`, `RenomearCategoria`.
- Infrastructure: endpoints, RequestValidators, `IRepositorioCategorias` implementado com EF Core, migrations, DI.
- Não depender da futura entidade Lançamento para criar as primeiras categorias.

**Não implementar** antes de definir as decisões bloqueantes. Pode-se implementar partes independentes sem fixar políticas de renomeação ou unicidade.
