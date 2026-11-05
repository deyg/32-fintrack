---
name: architecture-decision
description: Registra e revisa decisões de design e arquitetura de software em ADRs. Use ao escolher ou alterar padrões arquiteturais, camadas, dependências e políticas técnicas; não gere código.
---

# Architecture Decision

## Objetivo
Registrar decisões arquiteturais claras antes da implementação, sem gerar Solution ou código.

## Procedimento
1. Ler requisitos, restrições e ADRs existentes em `sdd/adr/`.
2. Identificar a decisão solicitada e alternativas realmente viáveis.
3. Explicitar consequências e trade-offs, sem inventar requisitos.
4. Criar ou atualizar um ADR em `sdd/adr/`, com: título, contexto, decisão, alternativas, consequências, **impacto nas camadas/tecnologias** e status (`Proposta` ou `Aceita`). Se alterar o posicionamento de uma tecnologia, indicar a atualização necessária no `STACK.md`, sem editá-lo por conta própria. Quando regras se aplicarem a Domain e borda HTTP, registrar preferência por fonte única no Domain e reutilização no RequestValidator; quando Result virar HTTP, exigir mapper compartilhado. Registrar a política aprovada: FluentValidation só na Infrastructure/Inbound para validar Requests HTTP; permitir validação antecipada das invariantes da entidade em RequestValidator de modo deliberado, mantendo o Domain como autoridade final e registrando o trade-off em ADR.
5. Não marcar como `Aceita` uma decisão ainda não aprovada pelo usuário.
6. Relatar qual ADR foi criado e o que ficou pendente.

## Limites
- Não executar scaffold, instalar bibliotecas nem implementar features.
- Não contrariar decisões aceitas sem propor um novo ADR que as substitua.
- Preservar as decisões já definidas: arquitetura hexagonal, Core com Domain e Application, e Infrastructure separada, salvo alteração explícita.
