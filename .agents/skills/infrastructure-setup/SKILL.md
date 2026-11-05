---
name: infrastructure-setup
description: Configura a infraestrutura técnica de uma Solution .NET existente: DI, FluentValidation, tratamento global de exceptions e ProblemDetails. Use depois do scaffold e do Core, sem implementar funcionalidades de negócio.
---

# Infrastructure Setup

## Objetivo
Conectar os componentes técnicos aprovados no `STACK.md`, sem escolher tecnologias novas.

## Procedimento
1. Ler `STACK.md`, especialmente a matriz `Tecnologia | Camada/local | Responsabilidade | Restrições`, os ADRs e os projetos existentes; verificar se Infrastructure hospeda uma Web API.
2. Instalar/configurar somente as tecnologias aprovadas e atribuídas à Infrastructure na matriz (por exemplo, persistência, OpenAPI, containerização quando previstas). Se houver ausência ou conflito de mapeamento, não escolher por conta própria: registrar pendência.
3. Configurar DI nativa e registrar implementações técnicas existentes. Contratos HTTP (`Request` e `Response`), e seus RequestValidators quando necessários, pertencem aos adaptadores inbound da Infrastructure; nunca expor Commands da Application como modelos HTTP.
4. Se FluentValidation estiver aprovado para validação antecipada HTTP, instalar a biblioteca apenas na Infrastructure, criar `RequestValidator` ao lado do Request e registrar `IValidator<Request>` na DI com validação explícita no endpoint. Não criar validators fictícios. Quando aprovado em ADR, usar validação antecipada para campos HTTP (inclusive nome obrigatório e comprimento); as invariantes continuam protegidas no Domain. Reutilizar fábrica/política de domínio para os mesmos critérios, evitando implementar a regra duas vezes. Em outros casos, validator é opcional.
5. Para Web API, configurar `IExceptionHandler`, `AddExceptionHandler`, `AddProblemDetails` e `UseExceptionHandler`.
6. Criar mapeamento centralizado reutilizável dos erros do Core para `ProblemDetails`: Validation=400, NotFound=404, Conflict=409, BusinessRule=422; exception inesperada=500. Não expor dados internos em produção.
7. Manter `CancellationToken` disponível em endpoints assíncronos (ASP.NET Core usa `RequestAborted`) e repassá-lo a UseCases, repositories e chamadas de I/O; não armazenar token em singleton ou modelos persistidos. Garantir que cancelamento solicitado pelo cliente não seja tratado como erro 500.
8. Se a stack utilizar EF Core com banco relacional, exigir migrations versionadas para evolução de esquema (salvo PoC descartável explicitamente autorizada). Não aplicar migrations automaticamente no startup de produção; definir o passo de atualização no deploy e o uso em testes de integração. Nunca trocar `EnsureCreated` por `Migrate` sem planejar a migração de bancos preexistentes.
9. Se houver API, validar inicialização e endpoint de saúde quando já definido; executar `dotnet restore` e `dotnet build`.
10. Informar os pacotes realmente adicionados e o resultado das verificações.

## Limites
- Infrastructure pode referenciar Core; nunca o inverso.
- Não introduzir EF Core, banco, mensageria ou autenticação sem decisão registrada.
- Não colocar código HTTP dentro do Core; não usar exceptions para erros de negócio esperados.
- Sem MediatR ou validação automática legada do MVC por padrão.
