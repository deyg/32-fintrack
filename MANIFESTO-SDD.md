# Manifesto do pacote SDD — FinTrack v0.1

**Tipo de entrega:** artefatos de especificação e orquestração, **sem alterações no código-fonte C#**.

## Documentos criados

- `PLAYBOOK.md` — fluxo SDD + Harness + commits por artefato.
- `sdd/requisitos/REQUISITOS.md` — objetivos, 15 RF, 24 RN, 10 RNF e pendências.
- `sdd/dominio/DOMINIO.md` — 3 entidades, dicionário de campos em português, relações e dados de exemplo.
- `sdd/decisoes/PENDENCIAS.md` — 10 decisões a ratificar.
- `sdd/specs/SPEC-F01-Categorias.md`
- `sdd/specs/SPEC-F02-Inativar-Categoria.md`
- `sdd/specs/SPEC-F03-Contas.md`
- `sdd/specs/SPEC-F04-Situacao-Classificacao-Conta.md`
- `sdd/specs/SPEC-F05-Registrar-Lancamento.md`
- `sdd/specs/SPEC-F06-Cancelar-Lancamento.md`
- `sdd/specs/SPEC-F07-Consultar-Lancamentos.md`
- `sdd/specs/SPEC-F08-Consolidar-Financas.md`
- `sdd/specs/SPEC-F09-Categorias-Iniciais.md`
- `sdd/progresso/MATRIZ-RASTREABILIDADE.md`
- `sdd/progresso/REGISTRO-COMMITS.md`
- `sdd/README.md`, `sdd/VALIDACAO-DOCUMENTAL.md`.

## Documentos alterados

- `AGENTS.md` — permite commits locais por artefato de código conforme a autorização do usuário, sem push, e exige consulta ao Playbook.
- `README.md` — links e aviso sobre evolução funcional ainda não implementada.

## Preservados

- 8 Skills do Harness v1.8.
- Código, migrations e testes da PoC v1.8.
- ADRs legados e a SPEC-001 da PoC, com alerta de evolução em F03.

## Evidência e limitação

Esta revisão valida estrutura documental, consistência básica e preservação dos arquivos-fonte. Não foi executado `dotnet build`, `dotnet test` nem commit Git. As SPECs são propostas; decisões de negócio pendentes impedem implementar automaticamente regras não ratificadas.
