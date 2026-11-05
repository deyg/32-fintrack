# SPEC-001 — Cadastrar Conta

Status: requisitos fornecidos pelo usuário; implementação de PoC autorizada.

## Objetivo
Criar uma conta por `POST /contas`, armazenando o cadastro no SQLite.

## Contrato

Request: `{ "nome": "Conta Corrente" }`.

Resposta de sucesso: HTTP 201 com `id` (GUID) e `nome`.

## Regras
- `nome` obrigatório e não pode conter somente espaços.
- `nome` tem no máximo 100 caracteres depois de `Trim()`.
- Não pode existir conta com nome duplicado.
- Duplicatas são comparadas sem diferenciar maiúsculas/minúsculas e após remover espaços externos (normalização `ToUpperInvariant`).
- O banco possui índice único em `NomeNormalizado`, para proteger concorrência.
- `CriarContaRequestValidator` usa FluentValidation na Infrastructure para validar antecipadamente nome obrigatório e comprimento após `Trim()`, devolvendo HTTP 400 com erros por campo.
- `Conta.Criar` permanece como proteção definitiva das invariantes, mesmo quando chamada fora de HTTP; a repetição na borda é deliberada (ADR-002).
- Falha esperada retorna Result/Result<T>, não exception para controle de fluxo.
- Cada etapa assíncrona repassa o `CancellationToken`.

## Status HTTP
- 201: cadastro concluído.
- 400: nome ausente, em branco ou longo demais.
- 409: nome normalizado duplicado.
- 500: erro interno inesperado, sem vazamento de detalhes.

## Testes de aceite
- validator HTTP rejeita nome nulo, em branco e longo demais, com erro para `nome`;
- validator HTTP aceita nome de 100 caracteres após `Trim()`;
- nome válido é cadastrado;
- nome de 100 caracteres é aceito;
- nome de 101 caracteres é rejeitado;
- nome vazio/em branco é rejeitado;
- `Conta Corrente` e `conta corrente` são tratados como duplicados;
- índice único SQLite impede inserção repetida, independentemente de verificações prévias.

## Fora do escopo
- Login/autorização, consulta/edição/exclusão de contas, migrations de produção, integrações externas.
