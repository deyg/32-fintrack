# SPEC-F09 — Inicializar Categorias

**Status:** Proposta · **Dependência:** F01 · **Bloqueios:** DP-02, DP-07.  
**Rastreabilidade:** RF-015; RN-001, RN-021, RN-024.

## História

Como usuário, quero iniciar o FinTrack com categorias financeiras comuns, podendo adicionar outras depois, sem duplicar registros ao reiniciar ou repetir a inicialização.

## Catálogo — exatamente 11 categorias

| Nome | Natureza |
|---|---|
| Moradia | DESPESA |
| Alimentação | DESPESA |
| Transporte | DESPESA |
| Saúde | DESPESA |
| Educação | DESPESA |
| Lazer | DESPESA |
| Assinaturas | DESPESA |
| Impostos | DESPESA |
| Salário | RECEITA |
| Rendimentos | RECEITA |
| Freelance | RECEITA |

## Política de inicialização

- Seed executada de maneira explícita no provisionamento, não em cada requisição HTTP.
- Idempotência: repetir não duplica registros. A estratégia de identificação deve seguir a regra de unicidade da Categoria aprovada em DP-02.
- Não sobrescrever nomes, naturezas ou estados alterados conscientemente pelo usuário sem regra explícita.
- Não criar Contas automaticamente, apenas Categorias.
- Contas e Lançamentos anteriores devem permanecer intactos.

## Cenários de aceite

1. Em banco vazio, seed cria 11 Categorias (8 DESPESA + 3 RECEITA).
2. Segunda execução não cria duplicatas e preserva relações.
3. Se houver categoria existente correspondente, a rotina não apaga/recria o ID.
4. Em falha parcial, operação é transacional ou repetível sem corromper o catálogo.
5. A aplicação mantém a possibilidade de criar categorias adicionais.

## Componentes

Dado inicial controlado no Infrastructure com mecanismo de seed aprovado; regras de Categoria permanecem no Domain. Testes com banco SQLite isolado e reaplicação.
