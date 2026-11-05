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

+| FT-F03-001 | F03 | src/FinTrack.Core/Domain/Enums/ClassificadorConta.cs | SPEC-F03 | validação do commit | Aprovado | 72739b254f170d098d6cec808187282315f51e71 | Concluído |
| FT-F03-002 | F03 | src/FinTrack.Core/Domain/Entities/Conta.cs | SPEC-F03 | validação do commit | Aprovado | 853d8250b9eb6d790303eef3cb6d79ea873c6278 | Concluído |
| FT-F03-003 | F03 | src/FinTrack.Core/Application/UseCases/Contas/CriarConta/CriarContaCommand.cs | SPEC-F03 | validação do commit | Aprovado | 59f6009113cace9c186dd7055e7503ad0e12a80a | Concluído |
| FT-F03-004 | F03 | src/FinTrack.Core/Application/UseCases/Contas/CriarConta/CriarContaOutput.cs | SPEC-F03 | validação do commit | Aprovado | a0c62d3015ae7a83c38830f1374c7a166615eca4 | Concluído |
| FT-F03-005 | F03 | src/FinTrack.Core/Application/UseCases/Contas/CriarConta/CriarContaUseCase.cs | SPEC-F03 | validação do commit | Aprovado | e9e8d3a136ba3a9ba711a362e732449e942920f7 | Concluído |
| FT-F03-006 | F03 | src/FinTrack.Infrastructure/Adapters/Inbound/Http/Contas/CriarContaRequest.cs | SPEC-F03 | validação do commit | Aprovado | 307a5d5d9a80856a10c2d4ac6369eca746da132f | Concluído |
| FT-F03-007 | F03 | src/FinTrack.Infrastructure/Adapters/Inbound/Http/Contas/CriarContaRequestValidator.cs | SPEC-F03 | validação do commit | Aprovado | 9b465b6aa9ebaacd2d2b46c54518712315b0a6d1 | Concluído |
| FT-F03-008 | F03 | src/FinTrack.Infrastructure/Adapters/Inbound/Http/Contas/ContasEndpoints.cs | SPEC-F03 | validação do commit | Aprovado | 3b60b183c257b5b15c80eb474e5db2c11956ae24 | Concluído |
| FT-F03-009 | F03 | src/FinTrack.Infrastructure/Persistence/FinTrackDbContext.cs | SPEC-F03 | validação do commit | Aprovado | f2344f8ec2111b2d9c30f1c26fe3b2266cb28d46 | Concluído |
| FT-F03-010 | F03 | src/FinTrack.Infrastructure/Persistence/Migrations/20261105122654_ExpandContasComCategoria.cs | SPEC-F03 | validação do commit | Aprovado | 3bf1ba69ec12d40274dcb02c480cd3a0ed3acf63 | Concluído |
| FT-F03-011 | F03 | src/FinTrack.Infrastructure/Persistence/Migrations/20261105122654_ExpandContasComCategoria.Designer.cs | SPEC-F03 | validação do commit | Aprovado | 6641953561ef5bbe7f0bbaa04d3214a1f6658540 | Concluído |
| FT-F03-012 | F03 | src/FinTrack.Infrastructure/Persistence/Migrations/FinTrackDbContextModelSnapshot.cs | SPEC-F03 | validação do commit | Aprovado | e983c6fc9fc01b098f66ad9a4e41a7f1e32f700d | Concluído |
| FT-F03-013 | F03 | src/FinTrack.Core/Domain/Ports/IContaRepository.cs | SPEC-F03 | validação do commit | Aprovado | 594c3cfaafb21a74234b61c4c178c7d059d2114a | Concluído |
| FT-F03-014 | F03 | src/FinTrack.Infrastructure/Persistence/ContaRepository.cs | SPEC-F03 | validação do commit | Aprovado | a13fbb601ac7502bfe4df8b13aff916871564941 | Concluído |
| FT-F03-015 | F03 | tests/FinTrack.Tests/CriarContaUseCaseTests.cs | SPEC-F03 | validação do commit | Aprovado | 31ad926ec6f7519f7f02eb3abc34255d62fee4fc | Concluído |
| FT-F03-016 | F03 | src/FinTrack.Core/Application/UseCases/Contas/ContaOutput.cs | SPEC-F03 | validação do commit | Aprovado | 0ec52d6cc2876d42c1e6c0e9df04bdec72fbbdf1 | Concluído |
| FT-F03-017 | F03 | src/FinTrack.Core/Application/UseCases/Contas/ConsultarConta/ConsultarContaUseCase.cs | SPEC-F03 | validação do commit | Aprovado | 5d7e2156286fc9aa1d57911d343c70ab9133a7d5 | Concluído |
| FT-F03-018 | F03 | src/FinTrack.Core/Application/UseCases/Contas/ListarContas/ListarContasUseCase.cs | SPEC-F03 | validação do commit | Aprovado | a0bff8118b9dc84ae87f3d1279b6fc0e6e37d457 | Concluído |
| FT-F03-019 | F03 | src/FinTrack.Core/Application/UseCases/Contas/RenomearConta/RenomearContaUseCase.cs | SPEC-F03 | validação do commit | Aprovado | 4aef024c4d4235dde066919fa64d8acbe053c879 | Concluído |
| FT-F03-020 | F03 | src/FinTrack.Infrastructure/Adapters/Inbound/Http/Contas/RenomearContaRequest.cs | SPEC-F03 | validação do commit | Aprovado | 973ee7d7f1898754e84cb447c4d9fabb07c72a90 | Concluído |
| FT-F03-021 | F03 | src/FinTrack.Infrastructure/Adapters/Inbound/Http/Contas/RenomearContaRequestValidator.cs | SPEC-F03 | validação do commit | Aprovado | b34409159631a7c8fbb4591a9e13c0b2ebe00922 | Concluído |
| FT-F03-022 | F03 | src/FinTrack.Infrastructure/Adapters/Inbound/Http/Contas/ContasEndpoints.cs | SPEC-F03 | validação do commit | Aprovado | 341134803b66642132a63b22a675c2783ad8b3ff | Concluído |
| FT-F03-023 | F03 | src/FinTrack.Infrastructure/Program.cs | SPEC-F03 | validação do commit | Aprovado | 25b8f1d37734865bbded02acef9d28f4d701ee39 | Concluído |
| FT-F03-024 | F03 | src/FinTrack.Core/Application/UseCases/Contas/CriarConta/CriarContaCommand.cs | SPEC-F03 | validação do commit | Aprovado | 12ead21745e8a2c423996dfc93f4272e2be24724 | Concluído |
| FT-F03-025 | F03 | src/FinTrack.Core/Application/UseCases/Contas/CriarConta/CriarContaUseCase.cs | SPEC-F03 | validação do commit | Aprovado | 9f21cb3713568f3db9f82282e783615457d380de | Concluído |
| FT-F03-026 | F03 | src/FinTrack.Infrastructure/Adapters/Inbound/Http/Contas/CriarContaRequest.cs | SPEC-F03 | validação do commit | Aprovado | f2c45394b5adfad85ca8045d182c1c545679aae8 | Concluído |
| FT-F03-027 | F03 | tests/FinTrack.Tests/CriarContaRequestValidatorTests.cs | SPEC-F03 | validação do commit | Aprovado | 07187bf5508b7c1df6876f5349c24a4bfb8a3c22 | Concluído |
| FT-F03-028 | F03 | tests/FinTrack.Tests/CriarContaUseCaseTests.cs | SPEC-F03 | validação do commit | Aprovado | 5e89dfdf7203135b6142884b01a3553ed56e91b4 | Concluído |
| FT-F03-029 | F03 | tests/FinTrack.Tests/MigrationsTests.cs | SPEC-F03 | validação do commit | Aprovado | c17ca17713c09b9cf480731fd4129328e6a97060 | Concluído |
| FT-F03-030 | F03 | tests/FinTrack.Tests/ContasHttpIntegrationTests.cs | SPEC-F03 | validação do commit | Aprovado | c5d1bc994ba03bf6a182ed14e1825c5aa3ad8efa | Concluído |
| FT-F03-031 | F03 | src/FinTrack.Core/Domain/Entities/Conta.cs | SPEC-F03 | validação do commit | Aprovado | 96b7deaf55d46287367d9c673ba1b8f26ccf1271 | Concluído |
| FT-F03-032 | F03 | tests/FinTrack.Tests/ContaTests.cs | SPEC-F03 | validação do commit | Aprovado | 9d3a11ac344ccc1050e8a69ea36f51976261c408 | Concluído |
| FT-F03-033 | F03 | tests/FinTrack.Tests/ContaRepositoryTests.cs | SPEC-F03 | validação do commit | Aprovado | 52e64fe9fb94df0a9cd816dd8fa0d2c372e239ba | Concluído |
| FT-F03-034 | F03 | src/FinTrack.Infrastructure/Persistence/Migrations/20261105122654_ExpandContasComCategoria.cs | SPEC-F03 | validação do commit | Aprovado | 3dec377557864607615d7e99172ed57d66610e9b | Concluído |
| FT-F03-035 | F03 | tests/FinTrack.Tests/MigracaoContasLegadasTests.cs | SPEC-F03 | validação do commit | Aprovado | 4d0b84e9d21e415058849e2a1aa12b4951133f78 | Concluído |
| FT-F02-001 | F02 | src/FinTrack.Core/Domain/Entities/Categoria.cs | SPEC-F02 | validação do commit | Aprovado | 71ab72b537da72bbd81e99a3ed406cf4cdb91f2b | Concluído |
| FT-F02-002 | F02 | src/FinTrack.Core/Application/UseCases/Categorias/InativarCategoria/InativarCategoriaUseCase.cs | SPEC-F02 | validação do commit | Aprovado | df598b87231ea5b50911b5edb83bb9b241c0f7ae | Concluído |
| FT-F02-003 | F02 | src/FinTrack.Core/Application/UseCases/Categorias/ReativarCategoria/ReativarCategoriaUseCase.cs | SPEC-F02 | validação do commit | Aprovado | 137c4cd253610155ac4bb6c97224e54683567303 | Concluído |
| FT-F02-004 | F02 | src/FinTrack.Infrastructure/Adapters/Inbound/Http/Categorias/CategoriasEndpoints.cs | SPEC-F02 | validação do commit | Aprovado | 1497408de799b5be4993d825650bf32c2247c851 | Concluído |
| FT-F02-005 | F02 | src/FinTrack.Infrastructure/Program.cs | SPEC-F02 | validação do commit | Aprovado | 31cc95617e772b48a1c1b279d4bea1cc50f52e4c | Concluído |
| FT-F02-006 | F02 | tests/FinTrack.Tests/CategoriaEstadoHttpIntegrationTests.cs | SPEC-F02 | validação do commit | Aprovado | c2ca9e84ddc02c9366c6c294d6f07f663cf554c2 | Concluído |
| FT-F04-001 | F04 | src/FinTrack.Core/Domain/Entities/Conta.cs | SPEC-F04 | validação do commit | Aprovado | e7a2a2d627e1460c2c50981d65db04294bfd0ae4 | Concluído |
| FT-F04-002 | F04 | src/FinTrack.Core/Application/UseCases/Contas/AlterarEstadoConta/AlterarEstadoContaUseCase.cs | SPEC-F04 | validação do commit | Aprovado | cb6f14561b79f04bee21b74e51faa2d518e5ce06 | Concluído |
| FT-F04-003 | F04 | src/FinTrack.Infrastructure/Adapters/Inbound/Http/Contas/ContasEndpoints.cs | SPEC-F04 | validação do commit | Aprovado | 0c37366689786017df07912cac015fda230c17ce | Concluído |
| FT-F04-004 | F04 | src/FinTrack.Infrastructure/Program.cs | SPEC-F04 | validação do commit | Aprovado | ff6799ab8c9fc7195744ba559985bf860e72cfb5 | Concluído |
| FT-F04-005 | F04 | tests/FinTrack.Tests/ContaEstadoHttpIntegrationTests.cs | SPEC-F04 | validação do commit | Aprovado | 57695d2504e326fe76a0541acdbf3ab3d9a284c9 | Concluído |
| FT-F05-001 | F05 | src/FinTrack.Core/Domain/Enums/SituacaoLancamento.cs | SPEC-F05 | validação do commit | Aprovado | 8218aa1c12a6019ecda447482971b641ba54f8a4 | Concluído |
| FT-F05-002 | F05 | src/FinTrack.Core/Domain/Entities/Lancamento.cs | SPEC-F05 | validação do commit | Aprovado | 614a68b09f5ef145984af7b526279b3398aaaec4 | Concluído |
| FT-F05-003 | F05 | src/FinTrack.Core/Domain/Ports/IRepositorioLancamentos.cs | SPEC-F05 | validação do commit | Aprovado | 06302764c56a82b30620e4d0c8132ae955ad2934 | Concluído |
| FT-F05-004 | F05 | src/FinTrack.Infrastructure/Persistence/FinTrackDbContext.cs | SPEC-F05 | validação do commit | Aprovado | 72b3d42309b36d32a8bb31a88aab00c31ee0557f | Concluído |
| FT-F05-005 | F05 | src/FinTrack.Infrastructure/Persistence/LancamentoRepository.cs | SPEC-F05 | validação do commit | Aprovado | f360cb22fdb1d11f1a3cf4ff46ceaff538ecd8a9 | Concluído |
| FT-F05-006 | F05 | src/FinTrack.Infrastructure/Persistence/Migrations/20261105160313_AddLancamentos.cs | SPEC-F05 | validação do commit | Aprovado | b9fc7651e5b3c1c08d4fb11179f8ce31c11f70b7 | Concluído |
| FT-F05-007 | F05 | src/FinTrack.Infrastructure/Persistence/Migrations/20261105160313_AddLancamentos.Designer.cs | SPEC-F05 | validação do commit | Aprovado | ba62d7d092ca6514a0b6b53f73e28bae3c16cbc8 | Concluído |
| FT-F05-008 | F05 | src/FinTrack.Infrastructure/Persistence/Migrations/FinTrackDbContextModelSnapshot.cs | SPEC-F05 | validação do commit | Aprovado | 362f43e526fb024923e408481b256aa45c549551 | Concluído |
| FT-F05-009 | F05 | src/FinTrack.Core/Application/UseCases/Lancamentos/LancamentoOutput.cs | SPEC-F05 | validação do commit | Aprovado | 5872f8f196840fdea013050559b6b912ab52ff03 | Concluído |
| FT-F05-010 | F05 | src/FinTrack.Core/Application/UseCases/Lancamentos/RegistrarLancamento/RegistrarLancamentoUseCase.cs | SPEC-F05 | validação do commit | Aprovado | f5fbd91724de617e15dbaaf6477fbea801b9abdd | Concluído |
| FT-F06-001 | F06 | src/FinTrack.Core/Application/UseCases/Lancamentos/CancelarLancamento/CancelarLancamentoUseCase.cs | SPEC-F06 | validação do commit | Aprovado | 15450a909f0135823e5c503437b9ebfc31ef6cab | Concluído |
| FT-F07-001 | F07 | src/FinTrack.Core/Application/UseCases/Lancamentos/ConsultarLancamentos/ConsultarLancamentosUseCase.cs | SPEC-F07 | validação do commit | Aprovado | 9d285a1f21ec3d68e0d4de9aa8cdae784f5fe369 | Concluído |
| FT-F08-001 | F08 | src/FinTrack.Core/Application/UseCases/Lancamentos/ConsolidarFinancas/ConsolidarFinancasUseCase.cs | SPEC-F08 | validação do commit | Aprovado | be9ad1ea59f4676a785678aa2328f1c01ade1b5d | Concluído |
| FT-F05-011 | F05 | src/FinTrack.Infrastructure/Adapters/Inbound/Http/Lancamentos/LancamentoRequests.cs | SPEC-F05 | validação do commit | Aprovado | d9739e8bb670d48bbc01981cfa75f0500a0e597c | Concluído |
| FT-F05-012 | F05 | src/FinTrack.Infrastructure/Adapters/Inbound/Http/Lancamentos/RegistrarLancamentoRequestValidator.cs | SPEC-F05 | validação do commit | Aprovado | a111aa32cec94da0472d68bfba2dfd63320e725d | Concluído |
| FT-F05-013 | F05 | src/FinTrack.Infrastructure/Adapters/Inbound/Http/Lancamentos/LancamentosEndpoints.cs | SPEC-F05 | validação do commit | Aprovado | 52fb2cb7a8d007478cadbbacba0a9cad92283471 | Concluído |
| FT-F05-014 | F05 | src/FinTrack.Infrastructure/Program.cs | SPEC-F05 | validação do commit | Aprovado | 90e5c209a60cd66ee23127c3036b54e1e45ebb86 | Concluído |
| FT-F05-015 | F05 | tests/FinTrack.Tests/MigrationsTests.cs | SPEC-F05 | validação do commit | Aprovado | 52503fdd13d176c0298ca4edd2ee161cc5b4b6e8 | Concluído |
| FT-F05-016 | F05 | tests/FinTrack.Tests/LancamentosHttpIntegrationTests.cs | SPEC-F05 | validação do commit | Aprovado | 38c282ddb00d73e2ace7aa681da4b6af4497bdf2 | Concluído |
| FT-F04-006 | F04 | src/FinTrack.Core/Application/UseCases/Contas/DefinirClassificador/DefinirClassificadorUseCase.cs | SPEC-F04 | validação do commit | Aprovado | bde78d3316bbe8b35baa82c3ed5a6c1bc449dcc4 | Concluído |
| FT-F04-007 | F04 | src/FinTrack.Infrastructure/Adapters/Inbound/Http/Contas/DefinirClassificadorRequest.cs | SPEC-F04 | validação do commit | Aprovado | 069d9508103705dcce1bb22347e81e4c93a8316e | Concluído |
| FT-F04-008 | F04 | src/FinTrack.Infrastructure/Adapters/Inbound/Http/Contas/ContasEndpoints.cs | SPEC-F04 | validação do commit | Aprovado | b9961e6b8a8f78221489df84bbcc1e48ffe5b834 | Concluído |
| FT-F04-009 | F04 | src/FinTrack.Infrastructure/Program.cs | SPEC-F04 | validação do commit | Aprovado | 31b42ef04b2259660e2c4dad6fcc9f610f176726 | Concluído |
| FT-F04-010 | F04 | tests/FinTrack.Tests/ClassificadorContaHttpIntegrationTests.cs | SPEC-F04 | validação do commit | Aprovado | 98980a41afb1b2658108a5a17e34969ad4ecaba3 | Concluído |
| FT-F08-002 | F08 | src/FinTrack.Infrastructure/Adapters/Inbound/Http/Lancamentos/LancamentosEndpoints.cs | SPEC-F08 | validação do commit | Aprovado | fbbe5c07620cee7e638f19f6ab6012100baae8c1 | Concluído |
| FT-F08-003 | F08 | tests/FinTrack.Tests/LancamentosHttpIntegrationTests.cs | SPEC-F08 | validação do commit | Aprovado | 1a3d1c15c4479a20f42fc94573f8125c0c3f6d64 | Concluído |
| FT-F03-036 | F03 | src/FinTrack.Core/Domain/Entities/Conta.cs | SPEC-F03 | validação do commit | Aprovado | aab3f43f371444d6f01b2a59500221cb82990620 | Concluído |
| FT-F03-037 | F03 | src/FinTrack.Core/Application/UseCases/Contas/ClassificarContaLegada/ClassificarContaLegadaUseCase.cs | SPEC-F03 | validação do commit | Aprovado | 7e430e17c61bccd05c399fd17212370901c15016 | Concluído |
| FT-F03-038 | F03 | src/FinTrack.Infrastructure/Adapters/Inbound/Http/Contas/ClassificarContaLegadaRequest.cs | SPEC-F03 | validação do commit | Aprovado | 73d3007ef479cf7fe8e1aa134d3b19bcc7c1882e | Concluído |
| FT-F03-039 | F03 | src/FinTrack.Infrastructure/Adapters/Inbound/Http/Contas/ContasEndpoints.cs | SPEC-F03 | validação do commit | Aprovado | c7294e91fd0d417f52685a899d3fbcbd8b71cff6 | Concluído |
| FT-F03-040 | F03 | src/FinTrack.Infrastructure/Program.cs | SPEC-F03 | validação do commit | Aprovado | 314b7731addf719c5a53ba02edff950869048178 | Concluído |
| FT-F03-041 | F03 | tests/FinTrack.Tests/ContaTests.cs | SPEC-F03 | validação do commit | Aprovado | c0f8b67c2d182a8b97dfe2347c6e61adfde2b977 | Concluído |

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
