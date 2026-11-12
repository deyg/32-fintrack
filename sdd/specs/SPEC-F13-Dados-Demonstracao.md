# SPEC-F13 — Inicializar Dados de Demonstração

**Status:** Aprovada em 2026-11-12 · **Dependência:** F09 e migrations do modelo atual · **Decisão:** ADR-012.
**Rastreabilidade:** RF-016; RN-001 a RN-007, RN-025 e RN-026.

## História

Como usuário que inicia o FinTrack pelo Docker Compose, quero encontrar Contas
e Lançamentos fictícios vinculados às Categorias iniciais, para visualizar os
recursos da aplicação sem cadastrar tudo manualmente.

## Escopo

- Executar a seed F09 de Categorias e, em seguida, criar as Contas e os
  Lançamentos deste catálogo.
- Rodar por serviço efêmero explícito do Compose, depois das migrations e antes
  da API. Não executar a carga no startup normal da API.
- Aplicar também a um banco persistente já existente uma única vez, sem alterar
  dados preexistentes.
- Manter F09 inalterada: ela continua criando somente as onze Categorias.

## Contas iniciais

| Nome | Categoria | Natureza | Classificador |
|---|---|---|---|
| Aluguel | Moradia | DESPESA | OF |
| Mercado | Alimentação | DESPESA | OV |
| Energia | Moradia | DESPESA | OV |
| Transporte por aplicativo | Transporte | DESPESA | NOV |
| Salário mensal | Salário | RECEITA | `null` |
| Serviços freelance | Freelance | RECEITA | `null` |

Todas as Contas nascem ativas.

## Lançamentos iniciais

`D` é a data civil UTC do primeiro provisionamento, lida uma vez de
`TimeProvider.System`. As datas são persistidas e não mudam em reinicializações.

| Descrição | Conta | Valor | Data de realização |
|---|---|---:|---|
| Aluguel | Aluguel | 1800,00 | D − 5 dias |
| Compras do mês | Mercado | 620,50 | D − 4 dias |
| Conta de energia | Energia | 180,45 | D − 3 dias |
| Corridas | Transporte por aplicativo | 42,90 | D − 2 dias |
| Recebimento mensal | Salário mensal | 5000,00 | D − 1 dia |
| Trabalho freelance | Serviços freelance | 900,00 | D |

Todos os lançamentos são realizados e têm valores positivos com até duas casas
decimais.

## Regras

1. Identificar a carga por `fintrack-demo-v1`, armazenado em uma tabela técnica
   `cargas_iniciais` com chave única.
2. Se o identificador já existir, terminar com sucesso sem inserir, alterar ou
   recriar Contas, Lançamentos ou Categorias. Isso se mantém mesmo se registros
   de demonstração tiverem sido editados ou removidos.
3. Executar Contas, Lançamentos e gravação do marcador em uma transação única.
   O marcador só existe se todas as inserções forem concluídas.
4. Antes de inserir, exigir as Categorias necessárias existentes, ativas e com
   a natureza descrita acima.
5. Se houver Conta com nome normalizado igual a qualquer Conta do catálogo,
   falhar explicitamente e reverter a transação. Não reutilizar, renomear ou
   sobrescrever Conta existente.
6. Falhas não são convertidas em sucesso e não deixam marcador parcial.
7. Uma futura carga precisa de nova versão aprovada; não reparar nem reconciliar
   automaticamente uma versão anterior.
8. A execução é exclusiva do Docker Compose local, mesmo que o serviço use
   `ASPNETCORE_ENVIRONMENT=Production`. Outros deploys Production e o startup da
   API não executam a carga.

## Critérios de aceite

1. Banco novo: F09 cria exatamente onze Categorias; a carga cria seis Contas,
   seis Lançamentos e um marcador.
2. Segunda execução: nenhuma entidade ou valor é duplicado; os dados editados
   ou removidos depois da primeira execução permanecem como estão.
3. Banco existente: Contas e Lançamentos anteriores são preservados.
4. Colisão de nome, Categoria inativa/ausente ou natureza incompatível:
   inicialização falha, transação é revertida e marcador não é gravado.
5. Lançamentos têm as datas relativas a D e D é estável após reinício.
6. Compose aguarda migrations e inicialização bem-sucedidas antes de iniciar a
   API; falha na carga impede a API de iniciar.
7. F09 continua independente e cria somente Categorias.

## Componentes

- Infrastructure/Persistence: entidade técnica `CargaInicial`, mapeamento EF
  Core, migration e `InicializadorDadosDemonstracao`.
- Infrastructure/Program: argumento explícito de execução da F09 e da F13.
- Docker Compose: serviço efêmero de inicialização em sequência após migrations.
- Tests: SQLite isolado, incluindo idempotência e rollback.
