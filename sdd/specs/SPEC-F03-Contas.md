# SPEC-F03 — Gerenciar Contas

**Status:** Proposta · **Dependências:** F01 e F09 (catálogo) · **Bloqueios:** DP-01, DP-03, DP-07, DP-08, DP-09.  
**Rastreabilidade:** RF-006, RF-007; RN-002, RN-004, RN-005, RN-006, RN-008, RN-009, RN-010, RN-015.

## História

Como usuário, quero criar rubricas reutilizáveis (Contas) dentro de Categorias para classificar minhas receitas e despesas.

## Contratos propostos

| Método | Rota | Operação |
|---|---|---|
| POST | `/contas` | Cadastrar |
| GET | `/contas` | Listar e filtrar por categoria/ativa |
| GET | `/contas/{id}` | Consultar |
| PATCH | `/contas/{id}/nome` | Renomear, com política histórica aprovada |

Request POST proposto:

```json
{"categoriaId":"<id-da-categoria>","nome":"Aluguel","classificador":"OF"}
```

Resposta de criação: 201 + `{id,categoriaId,nome,classificador,ativa}`. Conta de receita envia `classificador: null`.

## Regras

- Categoria deve existir e estar ativa para criar Conta.
- `NomeConta` é a fonte de validade de nome: obrigatório, `Trim()`, 1–100 caracteres. O RequestValidator do HTTP pode reutilizar sua validação; Domain continua a proteger a entidade.
- Nomes duplicados rejeitados; comportamento **global atualmente na PoC** (DP-01 pode mudar a regra).
- Categoria de DESPESA requer exatamente OF/OV/NOF/NOV; Categoria de RECEITA requer `null`.
- Classificador pertence à Conta, nunca ao Lançamento.
- Conta criada inicia ativa (proposta). `categoriaId` e `classificador` devem ser persistidos no esquema.
- Não permitir renomear de modo a violar unicidade; políticas de renomeação histórica dependem de DP-03.

## Cenários de aceite

1. Conta de despesa OF válida retorna 201.
2. Conta de receita com classificador `null` retorna 201.
3. Despesa sem classificador e receita com classificador retornam erro adequado.
4. Categoria inexistente/inativa não aceita nova Conta.
5. Nome com 101 caracteres ou vazio retorna erro; nome com 100 é aceito.
6. Nomes `Aluguel` e ` aluguel ` conflitam pelo nome normalizado.
7. Concorrência de dois cadastros com mesmo nome resulta em somente uma linha no SQLite.
8. Consulta retorna os campos em português e preserva a associação Categoria–Conta.

## Atenção à PoC existente

A SPEC legada `SPEC-001-Cadastrar-Conta.md` implementa `Conta` com somente nome e ID e não possui `CategoriaId`, `Classificador`, `Ativa`. **F03 é evolução de contrato e migração**, não implementação paralela de uma segunda Conta. Registrar ADR/alteração de SPEC e criar migration para atualizar os dados existentes. Não renomear tabelas/colunas ou trocar PK silenciosamente.

## Componentes

Domain `Conta`, `NomeConta`, port para repositório e regras de classificação; Application `CriarContaComando`/caso de uso, consulta e renomeação; Infrastructure Requests HTTP, FluentValidation, EF Core, índice único e testes.
