---
name: solution-scaffold
description: Cria a estrutura inicial de uma Solution .NET com projetos Core e Infrastructure, pastas da arquitetura hexagonal e referências entre projetos. Use quando o usuário pedir para iniciar ou estruturar uma Solution, sem implementar funcionalidades.
---

# Solution Scaffold (.NET)

## Objetivo
Criar somente a estrutura inicial de uma Solution .NET, sem tomar decisões de negócio nem implementar features.

## Entradas
- **Nome da Solution**: informado pelo usuário.
- **Versão do .NET**: informada pelo usuário; se ausente, consultar a versão desejada ou aproveitar a versão já definida no projeto.
- **Estrutura**: usar a estrutura abaixo, salvo indicação diferente do usuário ou decisão aprovada no `STACK.md`/ADRs. Consultar o mapeamento de camadas, quando existir, sem instalar tecnologias.

## Estrutura padrão

```text
<SolutionName>/
├── <SolutionName>.slnx  # ou .sln, conforme o SDK
└── src/
    ├── <SolutionName>.Core/
    │   ├── Domain/
    │   │   ├── Entities/
    │   │   ├── ValueObjects/
    │   │   └── Ports/
    │   └── Application/
    │       └── UseCases/
    └── <SolutionName>.Infrastructure/
        ├── Adapters/
        │   ├── Inbound/
        │   └── Outbound/
        └── DependencyInjection/
```

## Instruções
1. Verificar o SDK instalado e inspecionar o diretório para evitar sobrescritas.
2. Criar a Solution com `dotnet new sln`.
3. Criar **Core** como Class Library e **Infrastructure** como ASP.NET Core Web API, salvo pedido explícito para usar Class Library.
4. Adicionar ambos os projetos à Solution.
5. Criar as pastas indicadas. Usar `.gitkeep` nas pastas que precisem ser preservadas vazias.
6. Configurar apenas a referência **Infrastructure → Core**.
7. Executar `dotnet restore` e `dotnet build`.
8. Informar a árvore criada, os comandos executados e o resultado do build.

## Restrições
- Não criar projetos adicionais sem solicitação.
- Não implementar entidades, casos de uso, endpoints de negócio, validators ou integrações.
- Não criar pasta genérica `DTOs` por padrão: contratos HTTP e eventuais RequestValidators ficam na Infrastructure; Commands e UseCases são criados por feature na Application.
- Não instalar bibliotecas como FluentValidation, EF Core ou MediatR: isso cabe a outras Skills.
- Não definir Result Pattern, tratamento de erros nem decisões arquiteturais além da estrutura solicitada.
- Não modificar arquivos existentes sem avaliar impactos.
- Não fazer commit sem autorização.
- Não declarar build aprovado sem executá-lo.

## Critérios de aceite
- Solution e dois projetos criados.
- Pastas organizadas conforme a estrutura solicitada.
- Infrastructure referencia Core; Core não referencia Infrastructure.
- `dotnet build` executado, com resultado registrado.
