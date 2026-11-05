# ADR-002 — Validação antecipada HTTP com FluentValidation

Status: Substituída pela ADR-003 (histórico preservado).

## Contexto
Precisamos usar FluentValidation na API sem deixar a entidade desprotegida.
Uma validação antecipada melhora erros HTTP, mas repete algumas invariantes.

## Decisão
- **Infrastructure / Inbound:** `CriarContaRequestValidator` com FluentValidation
  valida obrigatoriedade e tamanho do nome no Request, antes do UseCase.
- **Domain:** `Conta.Criar` continua como autoridade final sobre as invariantes.
  A validação da borda **não substitui** a validação do domínio.
- **Application:** Command e UseCase sem dependência de FluentValidation.
- **Persistence:** índice único no SQLite para impedir duplicidades concorrentes;
  conflito conhecido vira `Result` de negócio, não HTTP 500.
- **HTTP:** erros de Request retornam 400 via `ValidationProblem`; erros de
  domínio são traduzidos na Infrastructure.

## Restrições
- Duplicação mínima é aceita **somente** para validação antecipada do
  contrato HTTP, de forma explícita e documentada.
- Manter mesmas condições semânticas na borda e no Domain (ex.: tamanho após `Trim`).
- Nenhuma dependência de FluentValidation é permitida no Core.
- Não fazer validação de unicidade apenas em FluentValidation: banco é a
  garantia final.

## Consequências
- Testar tanto o validator HTTP quanto a entidade e unicidade SQLite.
- A partir da ADR-003, a borda reutiliza a validação de NomeConta, sem duplicar a definição da regra.
- Outros adaptadores (mensageria, CLI) continuam seguros pelo Domain.
