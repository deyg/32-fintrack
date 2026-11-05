# SPEC-F05 — Registrar Lançamento

**Status:** Proposta · **Dependências:** F03/F04 · **Bloqueios:** DP-06, DP-07, DP-09, DP-10.  
**Rastreabilidade:** RF-010; RN-007, RN-011, RN-012, RN-016, RN-020, RN-022.

## História

Como usuário, quero registrar uma receita que recebi ou uma despesa que paguei, vinculando-a à Conta apropriada, para compor meu histórico.

## Contrato proposto

`POST /lancamentos`:

```json
{
  "contaId": "<id-da-conta>",
  "valor": 1800.00,
  "dataRealizacao": "2026-10-06",
  "descricao": "Aluguel de outubro"
}
```

Resposta: 201 + `{id,contaId,valor,dataRealizacao,descricao,situacao}`. Não retornar/armazenar natureza e classificador duplicados como propriedades do Lançamento.

## Regras

- Valor obrigatório e maior do que zero, enviado em número decimal, não string formatada `R$`.
- Data obrigatória, representa realização. Política de data futura/`DateOnly`/timezone requer DP-06/DP-10.
- Conta vinculada deve existir e estar ativa; avaliar também categoria inativa (DP-05).
- Iniciar Situação como `REALIZADO` (proposta derivada da operação).
- Descrição opcional; limite e trimming dependem de decisão.
- Natureza é derivada de `Conta.Categoria.Natureza`; Classificador da Conta.
- Não gerar lançamentos futuros, parcelados ou recorrentes automaticamente no MVP.

## Cenários de aceite

1. Registrar receita válida retorna 201, vinculada à Conta de receita.
2. Registrar despesa válida retorna 201, vinculada à Conta de despesa.
3. Valor zero ou negativo retorna 400/422 conforme camada e contrato aprovado.
4. Conta inexistente retorna 404; Conta inativa retorna falha esperada.
5. Data ausente/fora da política de realização retorna erro previsível.
6. Persistência conserva associação, quantidade decimal e data.
7. Não há colunas `natureza` e `classificador` em `lancamentos`.

## Componentes

Domain `Lancamento.Registrar`, Application caso de uso `RegistrarLancamento`, Infrastructure endpoint, EF Core e migration, testes de precisão e integridade referencial.
