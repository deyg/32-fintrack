# Registro de commits e validações — FinTrack

**F01:** implementação iniciada após aprovação das decisões em 2026-11-05.

> Preencher somente com SHAs reais observados no Git. Não inventar hashes nem afirmar execução de testes não realizados.

| Passo | Feature | Artefato de código | Requisito/Regra | Comando de validação | Resultado | Commit SHA | Situação |
|---|---|---|---|---|---|---|---|
| FT-F01-001 | F01 | `NaturezaCategoria.cs` | RN-001 | `dotnet build` | Aprovado | `c6136dcfd848285827dd68181d002ae6a7ba37b1` | Concluído |
| FT-F01-002 | F01 | `NomeCategoria.cs` | RN-021 | `dotnet build` | Aprovado | `4d64d54e5a5bac30f8b0cfc641d84f09eb2d632a` | Concluído |
| FT-F01-003 | F01 | `Categoria.cs` | RF-001/RF-003 | `dotnet build` | Aprovado | `31dcfb9f85a6f71ab1a14b2a9a540a0c92b66ecf` | Concluído |
| FT-F01-004 | F01 | `IRepositorioCategorias.cs` | RF-001–RF-003 | `dotnet build` | Aprovado | `319082afda6c5b94bccdd9715dbee94731b38e4f` | Concluído |
| FT-F01-005 | F01 | `CategoriaOutput.cs` | RF-001–RF-003 | `dotnet build` | Aprovado | `a27fedc0b829f0c8bca6477cb0be8a1dafa5e0e3` | Concluído |
| FT-F01-006 | F01 | `CriarCategoriaUseCase.cs` | RF-001 | `dotnet build` | Aprovado | `22215db9e3fd627e29c5b06b18b050233544cfab` | Concluído |
| FT-F01-007 | F01 | `ConsultarCategoriaUseCase.cs` | RF-002 | `dotnet build` | Aprovado | `aa520499a392222539c4abdf1f4534af604d5a38` | Concluído |
| FT-F01-008 | F01 | `ListarCategoriasUseCase.cs` | RF-002 | `dotnet build` | Aprovado | `93dad0b0e5cac788db9f57ba94b300a47020f9e1` | Concluído |
| FT-F01-009 | F01 | `RenomearCategoriaUseCase.cs` | RF-003 | `dotnet build` | Aprovado | `e2420b3eabc95ed6ac95446fa549d50a91231e78` | Concluído |
| FT-F01-010 | F01 | `CriarCategoriaRequest.cs` | RF-001 | `dotnet build` | Aprovado | `f3be64e9cc194dd63099ac664e4c0b6b04e3b782` | Concluído |
| FT-F01-011 | F01 | `CriarCategoriaRequestValidator.cs` | RN-001/RN-021 | `dotnet build` | Aprovado | `5809a0726fd5273728e9a5ed5f43a5760aae0446` | Concluído |
| FT-F01-012 | F01 | `RenomearCategoriaRequest.cs` | RF-003 | `dotnet build` | Aprovado | `30c551bcc36be6215b3970d8cfea43dbd9302dd5` | Concluído |
| FT-F01-013 | F01 | `RenomearCategoriaRequestValidator.cs` | RN-021 | `dotnet build` | Aprovado | `61b2eb8e9df7a94ce3f010d7494eac675341d376` | Concluído |
| FT-F01-014 | F01 | `FinTrackDbContext.cs` | RN-010/RN-021 | `dotnet build` | Aprovado | `b6f847223a9d51ee20b18c88863d24a901d930b3` | Concluído |
| FT-F01-015 | F01 | `CategoriaRepository.cs` | RF-001–RF-003 | `dotnet build` | Aprovado | `90fc155840f59a5ce1b37464f4286f7a39edc70a` | Concluído |
| FT-F01-016 | F01 | `CategoriasEndpoints.cs` | RF-001–RF-003 | `dotnet build` | Aprovado | `bda8168a4ce3bf9eaa5368a3c3871b6855cd5e38` | Concluído |
| FT-F01-017 | F01 | `Program.cs` | RF-001–RF-003 | `dotnet build` | Aprovado | `d791ce77864c00a71006c3cb90353b6b4d61f31d` | Concluído |
| FT-F01-018 | F01 | `20261105120623_AddCategorias.cs` | RN-010/RN-021 | `dotnet build` | Aprovado | `bdbc82b7b40c6e1a7a754e9187d9ea8a20a8d572` | Concluído |
| FT-F01-019 | F01 | `20261105120623_AddCategorias.Designer.cs` | RN-010 | `dotnet build` | Aprovado | `2b10c728259b1a96a5c39c81b602acc56a2a83ab` | Concluído |
| FT-F01-020 | F01 | `FinTrackDbContextModelSnapshot.cs` | RN-010 | `dotnet ef migrations has-pending-model-changes` | Sem mudanças pendentes | `2fdc426cd6051cc7678843ab1257635153399a23` | Concluído |
| FT-F01-021 | F01 | `FinTrackApiFactory.cs` | RNF-006 | teste HTTP legado | 9 aprovados | `f01e65e1469123ab723cface6e39eedfdfdff3b2` | Concluído |
| FT-F01-022 | F01 | `MigrationsTests.cs` | RNF-007 | teste de migration | 1 aprovado | `f42fc3fd8ee1262ffd3d0906570a2ddd42d1a3cc` | Concluído |
| FT-F01-023 | F01 | `CategoriasEndpoints.cs` | DP-09 | `dotnet build` | Aprovado | `6a218beef5b59b6f14c7ff5aa18dfabf0ae4a5f3` | Concluído |
| FT-F01-024 | F01 | `CategoriaTests.cs` | RN-001/RN-021 | filtro `CategoriaTests` | 5 aprovados | `c0839135c2d44e6f65d89e5ba8a8b1a2d9a19c41` | Concluído |
| FT-F01-025 | F01 | `CategoriasUseCasesTests.cs` | RF-001–RF-003 | filtro `CategoriasUseCasesTests` | 2 aprovados | `462a5e40ca68b016da7a6b86cbe6b3f0302661fd` | Concluído |
| FT-F01-026 | F01 | `CategoriaRepositoryTests.cs` | RN-010/RN-021 | filtro `CategoriaRepositoryTests` | 1 aprovado | `5617fdb1ee9bc89d334792e636e9e904e74aa2d8` | Concluído |
| FT-F01-027 | F01 | `CategoriasHttpIntegrationTests.cs` | RF-001–RF-003 | filtro `CategoriasHttpIntegrationTests` | 2 aprovados | `498d91b68db8c75855a13f4e6faf58c6d7f31739` | Concluído |
| FT-F01-028 | F01 | `CategoriasEndpoints.cs` | DP-08 | teste HTTP F01 | 2 aprovados | `0a71f95a08844cf5ec9ff1c218a75edf989e35af` | Concluído |
| FT-F09-001 | F09 | `InicializadorCategorias.cs` | RF-015/RN-024 | `dotnet build` | Aprovado | `58b90a02dcf38659a09986fb896b40fcd17d73c5` | Concluído |
| FT-F09-002 | F09 | `Program.cs` | RF-015 | `dotnet build` | Aprovado | `b5745c1be8678f3a23bbdd281cef404cae3d382f` | Concluído |
| FT-F09-003 | F09 | `InicializadorCategoriasTests.cs` | RF-015/RN-024 | filtro `InicializadorCategoriasTests` | 2 aprovados | `32fa77eef9ade9164a0b09a211564aa2414c3f2c` | Concluído |

## Modelo de registro por passo

```markdown
### FT-F03-001 — <artefato>
- SPEC / RF / RN: ...
- Arquivo alterado: ...
- Mudança: ...
- Verificação executada: `dotnet ...`
- Resultado verificável: ...
- Commit: `<SHA real>`
- Estado: CONCLUÍDO | BLOQUEADO | NÃO EXECUTADO
```

## Política

- Um artefato de código por commit, após validação compatível com o passo.
- SHAs são obtidos do Git local ou do repositório efetivo, nunca estimados.
- Se não houver SDK/ambiente, registrar **NÃO EXECUTADO**, não aprovação.
- Se um passo depender de decisões de negócio ainda pendentes, registrar **BLOQUEADO**, não gerar commit para mascarar a pendência.
- Nenhum push automático.
