# ADR-012 — Carga única de dados de demonstração no Docker Compose

**Status:** Aceita em 2026-11-12 por aprovação explícita do usuário.

## Contexto

Uma instalação nova do FinTrack fica sem Contas e Lançamentos, embora o Docker
Compose já aplique migrations e mantenha o SQLite em volume persistente. A F09
aprovada inicializa exclusivamente as onze Categorias e não deve ser ampliada
para criar Contas. Os requisitos aprovam uma carga fictícia separada para
visualização da aplicação.

## Decisão

- Manter F09 e seu catálogo de onze Categorias inalterados. Uma etapa de
  inicialização, distinta das migrations, executa a F09 e depois a carga
  `fintrack-demo-v1`.
- Executar a carga apenas em serviço explícito do Docker Compose local, depois
  de `migracoes` e antes de `api`. O serviço roda mesmo com
  `ASPNETCORE_ENVIRONMENT=Production`; isso não ativa a carga em outros deploys
  Production nem no startup da API.
- Persistir o marcador de conclusão em `cargas_iniciais`, no mesmo SQLite
  persistente, com identificador único por versão do conjunto.
- Criar Contas, Lançamentos e marcador em uma transação. Não reutilizar ou
  sobrescrever contas existentes. Colisão de nome, Categoria ausente/inativa ou
  natureza incompatível causa falha explícita e rollback, sem marcador.
- Se o identificador já existir, a inicialização não insere, altera ou recria
  registros, mesmo se editados ou removidos. Uma nova carga exige versão e
  aprovação próprias.
- Calcular as datas dos lançamentos uma única vez usando a data civil UTC do
  `TimeProvider` no primeiro provisionamento; persistir os valores.

## Alternativas consideradas

- Inserir dados em migration: rejeitada porque mistura evolução do esquema com
  dados de demonstração e não permite distinguir execução única da carga.
- Executar a carga em todo startup da API: rejeitada por recriar dados removidos
  e acoplar mutação de dados à inicialização de cada instância.
- Reconciliar dados em cada inicialização: rejeitada por sobrescrever mudanças
  do usuário.
- Reutilizar silenciosamente Contas existentes: rejeitada para não assumir
  propriedade sobre cadastros do usuário.

## Consequências e impacto

- **Infrastructure/Persistence:** tabela técnica `cargas_iniciais`, migration
  versionada, inicializador transacional e idempotente.
- **Infrastructure/Program:** modo explícito de execução da carga; não executado
  durante o startup HTTP normal.
- **Docker Compose:** novo serviço efêmero `inicializacao`, dependente de
  conclusão bem-sucedida de `migracoes`; a API depende de ambos.
- **Core:** sem mudanças; permanecem válidas as invariantes de Conta, Categoria
  e Lançamento.
- **Stack:** reutiliza EF Core/SQLite, `TimeProvider` e Docker Compose existentes;
  não adiciona pacotes.
- **Testes:** cobertura com SQLite para primeira aplicação, repetição, colisão
  com rollback, categoria incompatível e persistência do marcador.

## Limites

Esta decisão não altera o esquema de negócio de Contas e Lançamentos, não cria
novas Categorias além das onze da F09 e não define qualquer carga automática em
ambientes diferentes do Docker Compose local.
