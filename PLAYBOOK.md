# FINTRACK — PLAYBOOK SDD + CODEX + HARNESS

**Versão:** 0.1 · **Estado:** pronto para revisão; execução condicionada às decisões bloqueantes das SPECs.  
**Harness:** v1.8 (8 Skills). **Plataforma atual da PoC:** .NET 10, ASP.NET Core Minimal API, EF Core, SQLite, FluentValidation, xUnit.  
**Regra mandatória acordada:** **um commit Git por artefato de código construído ou alterado**, acompanhado de verificação e SHA real.

## 1. Missão

Evoluir a PoC de cadastro simples de Conta para o FinTrack de três entidades (`Categoria`, `Conta`, `Lancamento`) **sem perder histórico nem violar o domínio**, implementando uma feature por vez a partir de SPEC revisada, com testes, auditoria e rastreabilidade.

## 2. Fontes de verdade e precedência

1. Regras de negócio e aprovações atuais: `sdd/requisitos/REQUISITOS.md`, `sdd/dominio/DOMINIO.md` e decisões ratificadas.
2. Especificação da feature: `sdd/specs/SPEC-Fxx-*.md`.
3. Decisões arquiteturais aceitas: `sdd/adr/`.
4. Mapa de stack e localização das tecnologias: `STACK.md`.
5. Procedimentos reutilizáveis: `.agents/skills/*/SKILL.md`, acionados por `$feature-orchestrator`.
6. Instruções do agente no repositório: `AGENTS.md`.

Em divergência material: **não adivinhar, não aprovar silenciosamente, registrar bloqueio**. A SPEC legada `SPEC-001-Cadastrar-Conta.md` representa a PoC anterior e não substitui F03.

## 3. Sequência das features

| Ordem | SPEC | Feature | Pré-requisito | Prontidão |
|---|---|---|---|---|
| 1 | F01 | Gerenciar Categorias | Decisões DP-02/03/07/08/09 aplicáveis | Proposta |
| 2 | F09 | Inicializar 11 Categorias | F01 + política de unicidade | Proposta |
| 3 | F03 | Gerenciar Contas | F01 + evolução da Conta legada; DP-01/03 | Proposta |
| 4 | F02 | Inativar Categoria | F01 + F03 (teste de bloqueio) | Proposta |
| 5 | F04 | Situação e Classificação da Conta | F03, DP-04/05 | Proposta |
| 6 | F05 | Registrar Lançamento | F03/F04, DP-06/10 | Proposta |
| 7 | F06 | Cancelar Lançamento | F05, DP-10 | Proposta |
| 8 | F07 | Consultar Lançamentos | F05/F06, filtros e paginação | Proposta |
| 9 | F08 | Consolidar Finanças | F05/F06/F07, regras históricas | Proposta |
| 10 | F11 | Documentar API com Scalar | ADR-010 | Aprovada |
| 11 | F12 | Containerizar API | ADR-011 | Aprovada |

**Importante:** a criação deste Playbook não transforma propostas em aprovações de negócio.

## 4. Fluxo de execução por feature — obrigatório

### P0 — Preparação

- Inspecionar branch, `git status`, código atual e migrations; nunca sobrescrever arquivos deliberadamente modificados.
- Ler SPEC, requisitos RF/RN, domínio, STACK, ADRs e pendências da feature.
- Verificar critérios de aceite, contratos e decisões bloqueantes. Se faltar decisão, registrar o motivo e avançar apenas no que não depende dela.
- Definir **lista ordenada de artefatos concretos de código** (ex.: `Categoria.cs`, port, caso de uso, Request, endpoint, configuração EF, migration, teste). Não juntar vários artefatos em um commit.

### P1 — Orquestração das Skills

Usar `$feature-orchestrator` como entrada; ele consulta as demais Skills na ordem adequada:

1. `architecture-decision` — se existir decisão nova ou divergente (condicional).
2. `stack-definition` — se stack/camada faltar ou mudar (condicional).
3. `solution-scaffold` — somente se não existir Solution (condicional).
4. `core-foundation` — somente se faltar base compartilhada (condicional).
5. `infrastructure-setup` — quando tecnologia ou DI precisar de preparo (condicional).
6. `feature-implementation` — implementa a SPEC aprovada.
7. `architecture-validation` — auditoria obrigatória após implementar.

### P2 — Baby steps e commits (obrigatório)

**Regra CMT-001:** para **cada artefato de código criado ou modificado**, realizar um passo verificável e **um commit próprio**. Inclui classes de domínio, casos de uso, repositórios, endpoints, arquivos de migrations, testes e configuração executável. Não agrupar artefatos distintos em um único commit por conveniência.

**Passos por artefato:**

1. Identificar ID do passo e artefato exato (`FT-F03-005 — Conta.cs`), com RF/RN/SPEC associados.
2. Implementar **somente esse artefato**, preservando build no estado do commit. Preparar dependências e stubs mínimos em passos anteriores quando necessário; não commitar código quebrado.
3. Executar validação pertinente disponível (`dotnet build`, testes da unidade, análise de referências; migrations com verificação do modelo); registrar comando, saída e eventuais falhas.
4. Se falhou, **não commitar como concluído**; corrigir, repetir validação. Se não for possível validar por indisponibilidade de ferramentas, registrar **BLOQUEADO / NÃO EXECUTADO** e não declarar o passo concluído.
5. `git add -- <caminho-exato>` para o artefato do passo. `git diff --cached` deve demonstrar somente o escopo esperado.
6. Fazer **um commit** com mensagem técnica e ID do passo, por exemplo `feat(domain): FT-F03-005 cria entidade Conta`.
7. Registrar SHA real (`git rev-parse HEAD`), arquivo, testes, SPEC/RN e resultado em `sdd/progresso/REGISTRO-COMMITS.md`.
8. Se dois arquivos formarem uma dependência inseparável que não pode ser comitada compilando separadamente, replanejar em incrementos compiláveis; qualquer exceção à granularidade exige autorização explícita, **não reduzir o rigor silenciosamente**.

**Sem `--no-verify`/commits vazios para contornar falhas. Sem commits misturados com alterações alheias. Sem `push`, merge, rebase destrutivo ou force-push sem autorização específica.**

Commits de documentação podem ser feitos separadamente; **não substituem** commits unitários de código. Para este pacote documental, ainda não houve implementação nova de código e, portanto, **não existem commits SHA gerados aqui**.

### Comandos mínimos de verificação (quando SDK disponível)

```bash
dotnet restore FinTrack.slnx
dotnet build FinTrack.slnx --no-restore
dotnet test FinTrack.slnx --no-build
```

Acrescentar testes de integração e verificação de migrações da feature. Registrar saídas reais, não resultados estimados.

### P3 — Critérios de entrega da feature

- Código e testes compatíveis com o estado aprovado da SPEC.
- Build e testes executados quando o SDK está disponível; nenhum teste declarado como passado por inspeção.
- Migrations versionadas e verificadas com ferramentas EF oficiais; sem `EnsureCreated` em produção.
- Testes de HTTP real com `WebApplicationFactory`, SQLite isolado e cenários 201/400/404/409/422 pertinentes.
- CancellationToken propagado até I/O; FluentValidation HTTP na Infrastructure; invariantes no Domain; `Result` independente de HTTP.
- Auditoria da Skill `architecture-validation` anexada ao progresso.
- **Lista de artefatos → um SHA por artefato → evidência** preenchida.
- Não marcar Done caso exista DP bloqueante, build não executado ou teste de aceite pendente.

## 5. Linguagem, modelo e contrato

- Tabelas/colunas: `categorias`, `contas`, `lancamentos`; `categoria_id`, `conta_id`, `data_realizacao`, etc.
- Entidades, propriedades e métodos de negócio em português sem acentos (`Categoria`, `Conta`, `Lancamento`, `Criar`, `Cancelar`).
- Contracts Request/Response na Infrastructure; comando e saída do UseCase na Application; Domain sem HTTP/FluentValidation/EF Core.
- Não confundir `Conta` (rubrica) com conta bancária.
- **A migração da PoC legada com Conta apenas nome para o novo modelo Categoria + Classificador exige planejamento**, sem descartar registros.

## 6. Exemplo de pedido ao Codex

```text
$feature-orchestrator

Evolua a SPEC-F03 — Gerenciar Contas.
Leia PLAYBOOK.md, sdd/requisitos/REQUISITOS.md, sdd/dominio/DOMINIO.md,
STACK.md, sdd/adr/ e sdd/decisoes/PENDENCIAS.md.
Não invente decisões pendentes; sinalize bloqueios antes de implementá-las.
Implemente em baby steps, com um commit por artefato de código,
valide cada passo e registre SHA real em sdd/progresso/REGISTRO-COMMITS.md.
Ao final, rode a Skill architecture-validation e entregue evidências.
Não faça push.
```

## 7. Checklist antes de concluir

- [ ] SPEC e decisões bloqueantes aceitas.
- [ ] Migrations e impacto sobre dados existentes analisados.
- [ ] Arquitetura e stack conformes.
- [ ] Testes de domínio/HTTP executados, não apenas criados.
- [ ] Um commit por artefato de código, com SHA registrado.
- [ ] Auditoria final realizada.
- [ ] Relatório indica conclusão, pendências ou bloqueios sem exagerar evidências.
