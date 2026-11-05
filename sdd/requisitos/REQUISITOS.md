# FinTrack — Requisitos do Produto e do MVP

**Versão:** 0.1 · **Estado:** proposta para revisão · **Idioma:** pt-BR  
**Fonte:** objetivos, modelo Categoria–Conta–Lançamento e decisões discutidas para o FinTrack.  
**Aprovação:** ainda não registrada para este documento; não inferir aprovação apenas pela geração.

## 1. Objetivo

Permitir que uma pessoa registre **receitas recebidas** e **despesas pagas**, organizadas em **categorias** e **contas reutilizáveis (rubricas)**, e consulte seus totais por período, conta, categoria, natureza e classificador. O resultado financeiro é a soma de receitas menos a soma de despesas realizadas não canceladas.

**Conta não representa necessariamente conta bancária.** Não haverá integração bancária nem saldo bancário no MVP.

## 2. Escopo

**Incluído:** cadastrar e administrar categorias e contas, cadastrar lançamentos realizados, cancelar lançamentos logicamente, consultar histórico, consolidar receitas, despesas e resultado, iniciar catálogo de categorias.  
**Fora do MVP:** autenticação multiusuário, conciliação bancária, contas bancárias, cartão de crédito, parcelas futuras, transferências, orçamento/projeções, importação OFX, moedas múltiplas e recorrência automática.

## 3. Vocabulário canônico

| Conceito | Descrição |
|---|---|
| Categoria | Agrupador financeiro de natureza **RECEITA** ou **DESPESA**. |
| Conta | Rubrica reutilizável vinculada a uma Categoria; não é instituição bancária. |
| Lançamento | Evento financeiro efetivamente realizado, sempre com valor positivo. |
| Natureza | Herdada da Categoria; não duplicada em Conta nem Lançamento. |
| Classificador | Definido na Conta de despesa: **OF** (Obrigatória Fixa), **OV** (Obrigatória Variável), **NOF** (Não Obrigatória Fixa), **NOV** (Não Obrigatória Variável). Para receitas, `null`. |
| Resultado financeiro | Total das receitas menos total das despesas no período. Não é saldo de conta bancária. |

## 4. Requisitos funcionais

| ID | Requisito | Feature | Prioridade |
|---|---|---|---|
| RF-001 | Cadastrar Categoria com nome e natureza | F01 | Alta |
| RF-002 | Listar e consultar Categorias | F01 | Alta |
| RF-003 | Renomear Categoria respeitando restrições históricas | F01 | Alta |
| RF-004 | Inativar Categoria sem perda histórica | F02 | Alta |
| RF-005 | Reativar Categoria, se política aprovada | F02 | Proposta |
| RF-006 | Cadastrar Conta vinculada à Categoria | F03 | Alta |
| RF-007 | Listar, consultar e renomear Contas | F03 | Alta |
| RF-008 | Ativar/inativar Conta | F04 | Alta |
| RF-009 | Definir e controlar classificador de despesa | F04 | Alta |
| RF-010 | Registrar Lançamento realizado, de valor positivo | F05 | Alta |
| RF-011 | Cancelar lançamento logicamente e impedir reativação | F06 | Alta |
| RF-012 | Consultar lançamentos por período, conta, categoria e classificador | F07 | Alta |
| RF-013 | Calcular receitas, despesas e resultado no período | F08 | Alta |
| RF-014 | Consolidar totais por conta, categoria e classificador | F08 | Alta |
| RF-015 | Inicializar catálogo de 11 Categorias, sem duplicar seed | F09 | Média |

## 5. Regras de negócio

| ID | Regra | Situação |
|---|---|---|
| RN-001 | Cada Categoria tem exatamente uma natureza: RECEITA ou DESPESA. | Definida |
| RN-002 | Cada Conta pertence a exatamente uma Categoria. | Definida |
| RN-003 | Uma Categoria agrupa zero ou mais Contas; uma Conta agrupa zero ou mais Lançamentos. | Definida |
| RN-004 | Contas de RECEITA possuem `Classificador = null`. | Definida |
| RN-005 | Contas de DESPESA exigem um dos classificadores OF/OV/NOF/NOV. | Definida |
| RN-006 | Uma Categoria de DESPESA pode ter Contas com classificadores diferentes. | Definida |
| RN-007 | Natureza e classificador não são copiados para Lançamento. | Definida |
| RN-008 | Nome de Conta é obrigatório, com no máximo 100 caracteres após remover espaços externos. | Definida na PoC |
| RN-009 | Nomes de Contas duplicados são proibidos considerando `Trim` + comparação sem caixa. | Definida na PoC; escopo global a confirmar |
| RN-010 | O banco deve proteger duplicidades com restrição única, além de validações de entrada. | Definida |
| RN-011 | Lançamento contém valor positivo (> 0) e data de realização. | Definida |
| RN-012 | Lançamento representa fato realizado, não previsão; data futura é recusada. | Proposta para ratificação |
| RN-013 | Lançamento cancelado permanece no histórico e não pode ser reativado. | Definida |
| RN-014 | Lançamentos cancelados não compõem totais nem resultado. | Proposta coerente com cancelamento |
| RN-015 | Categoria inativa não admite novas Contas. | Definida |
| RN-016 | Conta inativa não admite novos Lançamentos. | Proposta para ratificação |
| RN-017 | Alterações de natureza/classificador/categoria não podem reinterpretar o histórico financeiro. | Definida em princípio; política exata pendente |
| RN-018 | Exclusão em cascata não pode apagar histórico financeiro. | Proposta de integridade |
| RN-019 | O resultado = receitas realizadas válidas − despesas realizadas válidas no período. | Definida |
| RN-020 | Um valor financeiro é registrado positivo; a natureza deriva de Categoria. | Definida |
| RN-021 | Nome de Categoria é obrigatório e não deve repetir outra Categoria segundo política de unicidade a ratificar. | Proposta |
| RN-022 | Operações monetárias usam `decimal`; escala e arredondamento devem ser definidos antes de implementar. | Pendente |
| RN-023 | Inativar/reativar Conta ou Categoria não altera Lançamentos já registrados. | Proposta |
| RN-024 | A seed das 11 categorias é idempotente, sem sobrescrever cadastros existentes. | Definida |

## 6. Catálogo inicial de categorias (11)

| Nome | Natureza |
|---|---|
| Moradia | DESPESA |
| Alimentação | DESPESA |
| Transporte | DESPESA |
| Saúde | DESPESA |
| Educação | DESPESA |
| Lazer | DESPESA |
| Assinaturas | DESPESA |
| Impostos | DESPESA |
| Salário | RECEITA |
| Rendimentos | RECEITA |
| Freelance | RECEITA |

São categorias iniciais, não contas obrigatórias. O usuário pode criar categorias adicionais.

## 7. Requisitos não funcionais e de engenharia

| ID | Requisito | Verificação |
|---|---|---|
| RNF-001 | Arquitetura Hexagonal com `Core` e `Infrastructure` como projetos de produção. | Teste estático de referências. |
| RNF-002 | Identificadores de negócio em pt-BR: nomes de tabelas, colunas, entidades, atributos, métodos e campos públicos HTTP. | Revisão de nomenclatura. |
| RNF-003 | FluentValidation no adaptador HTTP; invariantes definitivas no Domain, preferindo regra compartilhada. | Testes HTTP + domínio. |
| RNF-004 | `Result` para falhas esperadas; `ProblemDetails` na borda e handler para falhas inesperadas. | Testes 400/404/409/422/500 apropriados. |
| RNF-005 | `CancellationToken` propagado nos métodos assíncronos até I/O. | Revisão das assinaturas e execução. |
| RNF-006 | SQLite/EF Core com migrations versionadas, sem `EnsureCreated` em produção. | Verificação de migrations e testes. |
| RNF-007 | Uma SPEC por feature, com critérios de aceite e testes. | Auditoria de rastreabilidade. |
| RNF-008 | **Um commit por artefato de código produzido ou alterado**, após validação possível; registrar SHA e evidências. | Histórico Git + relatório de progresso. |
| RNF-009 | A implementação não pode fazer push nem aprovar decisões pendentes silenciosamente. | Auditoria Git e ADR. |
| RNF-010 | Testes unitários de domínio e integração de API com banco isolado. | `dotnet test` executado e registrado. |

## 8. Decisões pendentes — não inventar no código

| Código | Decisão necessária | Sugestão de trabalho |
|---|---|---|
| DP-01 | Unicidade de nomes de Conta: global ou por Categoria? | Preservar unicidade global da PoC até decisão. |
| DP-02 | Unicidade e limite do nome de Categoria. | Nome normalizado e limite explícito, sujeito à aprovação. |
| DP-03 | Renomear Categoria/Conta com lançamentos: permitir e como auditar? | Bloquear mudanças sem regra autorizada. |
| DP-04 | Mudar Categoria/Classificador da Conta com histórico: quando permitir? | Bloquear quando houver lançamentos, inclusive cancelados, até decisão. |
| DP-05 | Reativação de Categoria/Conta: condições? | Reativação da Conta proposta; definir Categoria. |
| DP-06 | Precisão monetária, política de arredondamento e timezone/data de realização. | `decimal` e data local; detalhar antes de F05. |
| DP-07 | Identificadores SQL `int` (diagrama inicial) ou `Guid` (PoC existente)? | Preservar Guid durante migração, sujeito a ADR. |
| DP-08 | Paginação, ordenação e limites de consulta. | Definir antes de F07. |
| DP-09 | API: rotas e contratos definitivos das novas features. | SPECs contêm propostas; aprovar antes de implementar. |
| DP-10 | Validação de data futura e política de cancelamento (motivo, data). | Ratificar antes de F05/F06. |

## 9. Critério de prontidão (Definition of Ready)

Uma SPEC está pronta quando as regras relevantes, contrato, migração necessária, política histórica, testes e decisões pendentes bloqueantes estiverem definidos/aceitos. Status **Proposta** não autoriza o Codex a transformar hipótese em regra permanente.

**Documentos relacionados:** `sdd/dominio/DOMINIO.md`, `PLAYBOOK.md` e `sdd/specs/SPEC-Fxx-*.md`.
