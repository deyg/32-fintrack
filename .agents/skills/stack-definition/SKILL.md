---
name: stack-definition
description: Define e documenta a stack técnica de uma Solution em STACK.md, com base nas decisões aprovadas. Use quando o usuário definir ou revisar .NET, bibliotecas, banco, validações ou práticas de implementação.
---

# Stack Definition

## Objetivo
Manter um `STACK.md` curto, rastreável e legível pelas demais Skills.

## Procedimento
1. Ler a solicitação, o `STACK.md` existente, os ADRs e os projetos atuais.
2. Criar/atualizar na raiz do projeto o `STACK.md` com uma **matriz obrigatória**: `Tecnologia | Camada/local | Responsabilidade | Restrições`.
3. Para cada tecnologia aprovada, indicar onde deve ser usada e onde não deve; registrar versões conhecidas. Usar `A definir` quando faltar uma decisão, sem inventá-la.
4. Separar escolhas obrigatórias, opcionais e proibidas; vincular decisões relevantes aos ADRs.
5. Para ferramentas transversais (xUnit, Docker Compose, Git/GitHub), identificar `Tests`, `DevOps` ou `Ambiente`, sem forçá-las a Domain/Application/Infrastructure.
6. Detectar divergências entre matriz, ADRs e código existente; não alterar decisões aprovadas silenciosamente.
7. Mostrar as mudanças realizadas no `STACK.md`.

## Formato obrigatório do STACK.md
Manter uma tabela Markdown com estas quatro colunas (mesmo quando o projeto não usa todas as tecnologias abaixo):

| Tecnologia | Camada/local | Responsabilidade | Restrições |
|---|---|---|---|
| ASP.NET Core | Infrastructure / Inbound | Endpoints e contratos HTTP | Não usar no Core |
| FluentValidation | Infrastructure / Inbound / HTTP | Validar Requests antecipadamente quando aprovado | Não usar no Core; repetir invariantes somente com justificativa em ADR |
| EF Core + banco escolhido | Infrastructure / Outbound | Persistência e mapeamento | Core define ports, não usa EF Core |
| Result Pattern | Core / Domain e Application | Retornar falhas esperadas | Sem tipos HTTP no Core |
| ProblemDetails | Infrastructure / Inbound | Representar erros na API | Não usar no Core |

As linhas acima são **exemplos de posicionamento**, não autorização para instalar pacotes ou escolher banco. Outras tecnologias definidas pelo usuário devem entrar na matriz com localização e restrições apropriadas.

## Padrões já acordados (salvo nova decisão)
- Arquitetura Hexagonal .NET: Core (Domain + Application) e Infrastructure.
- FluentValidation exclusivamente na Infrastructure/Inbound para validação antecipada de Requests HTTP quando aprovada; deve reutilizar política/fábrica de domínio para invariantes compartilhadas quando possível (ADR), sem reproduzir expressões; Domain permanece autoridade final. Não criar validator automaticamente para toda feature.
- Result → ProblemDetails centralizado na Infrastructure, evitando mapeamento duplicado nos endpoints. Testes HTTP com WebApplicationFactory quando houver API e projeto de testes.
- Result Pattern simples (`Success`, `Failure`, `Match`), sem `Map` ou `Bind` inicialmente.
- DI nativa; ASP.NET Core `ProblemDetails` na fronteira HTTP; sem MediatR por padrão.
- Contratos HTTP (`Request`/`Response`) e seus eventuais `RequestValidator` nos adaptadores inbound da Infrastructure; `Command` e `UseCase` juntos por feature na Application. O adaptador converte `Request → Command`. Validação antecipada por FluentValidation não substitui invariantes do Domain.
- EF Core com persistência durável deve usar migrations versionadas e fluxo explícito de implantação; `EnsureCreated` apenas em PoC descartável documentada.
- `CancellationToken` nos métodos assíncronos de Application e Infrastructure e nas assinaturas de ports que representam I/O assíncrono. Propagar o mesmo token do endpoint até as operações de I/O (EF Core, HTTP, mensageria etc.). Não exigir token em métodos síncronos simples do Domain.

## Limites
- Não escolher banco, ORM, mensageria ou pacote extra por conta própria.
- Não gerar projetos, instalar dependências ou implementar funcionalidades.
