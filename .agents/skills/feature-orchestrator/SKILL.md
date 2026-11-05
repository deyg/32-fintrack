---
name: feature-orchestrator
description: Orquestra uma feature .NET orientada por SDD, aplicando em ordem as Skills de decisão, stack, scaffold, fundação, infraestrutura, implementação e validação, somente quando necessárias. Use quando o usuário pedir para construir uma feature completa.
---

# Feature Orchestrator (SDD)

## Objetivo
Conduzir uma feature ponta a ponta, reutilizando as outras Skills do Harness, sem duplicar suas regras nem implementar o código diretamente.

## Como utilizar as outras Skills
- Para cada etapa necessária, ler `.agents/skills/<nome>/SKILL.md` e seguir suas instruções no repositório atual. Se a Skill estiver instalada em outro local, localizar o arquivo antes de continuar.
- Referenciar uma Skill **não é** uma chamada de API nem garante ativação automática: executar explicitamente as instruções do arquivo, na ordem abaixo.
- Não executar uma etapa apenas porque a Skill existe. Se a etapa já estiver atendida, registrar `dispensada`.
- Se alguma Skill obrigatória não estiver acessível, informar a pendência em vez de simular sua execução.

## Fluxo de execução
1. **Diagnóstico:** ler a solicitação, SPEC em `sdd/specs/` (se existir), ADRs, `STACK.md`, estrutura da Solution e código relevante. Verificar o que já existe e o que falta.
2. **`architecture-decision` (condicional):** aplicar quando a feature requer uma decisão arquitetural nova ou alteração de ADR. Não aceitar decisões em nome do usuário.
3. **`stack-definition` (condicional):** aplicar quando o `STACK.md` não existe, está incompleto ou precisa registrar tecnologias/camadas já decididas. Não inventar escolhas.
4. **`solution-scaffold` (condicional):** aplicar somente se a Solution/projetos necessários não existirem e seu nome/versão estiverem definidos. Não recriar a estrutura existente.
5. **`core-foundation` (condicional):** aplicar se as abstrações genéricas aprovadas do Core ainda não estiverem implementadas.
6. **`infrastructure-setup` (condicional):** aplicar se a feature exigir configuração técnica ainda ausente, como DI, banco, mapeamento de erros ou documentação da API, desde que prevista no `STACK.md`.
7. **`feature-implementation` (obrigatória para implementar):** executar uma feature por vez, com base na SPEC e nas decisões aprovadas. Preservar `Request`/`RequestValidator` (se necessário) na Infrastructure, `Command`/`UseCase` na Application e invariantes no Domain, admitindo validação antecipada deliberada na borda quando registrada em ADR, com regras compartilhadas definidas no Domain e mapper HTTP comum na Infrastructure. Propagar `CancellationToken` nos fluxos assíncronos de I/O.
8. **`architecture-validation` (obrigatória após implementar):** executar auditoria, build, testes unitários e HTTP de integração aplicáveis. Se houver falha, encaminhar correção para a Skill responsável e repetir a validação; não declarar sucesso sem evidências.

## Regras de decisão
- Se não houver SPEC aprovada, usar apenas os requisitos explicitamente fornecidos para redigir/propor a SPEC mínima; solicitar aprovação antes de implementar o que ainda depende dessa aprovação.
- Decisão de arquitetura precede atualização de stack; configuração precede implementação; validação sucede implementação.
- A existência de um projeto não autoriza alterar a stack. Respeitar ADRs aceitos e a matriz `Tecnologia | Camada/local | Responsabilidade | Restrições` do `STACK.md`.
- Não adicionar bibliotecas, projetos, migrations, commits ou features não solicitados sem suporte nas decisões existentes.
- Não mascarar erros de build/teste nem alegar execução de Skill que não tenha sido efetivamente aplicada.

## Relatório final
Apresentar uma tabela concisa `Etapa | Executada/Dispensada/Bloqueada | Evidência`, arquivos criados/alterados, resultados reais de build/testes e pendências. Se houver bloqueio, apontar a próxima decisão necessária.
