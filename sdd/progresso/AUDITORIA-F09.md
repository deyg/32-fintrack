# Auditoria arquitetural — F09 Inicializar Categorias

**Data:** 2026-11-05  
**Resultado:** Conforme

- Seed localizada na Infrastructure, sem regra de persistência vazando para o Core.
- Execução explícita por `--inicializar-categorias`; não ocorre no startup comum nem por requisição.
- Operação transacional, idempotente pela política normalizada do Domain e sem sobrescrita.
- `CancellationToken` propagado até consultas, gravação e transação do EF Core.
- Banco vazio: 11 categorias, sendo 8 despesas e 3 receitas.
- Reaplicação: zero inclusões e IDs existentes preservados.
- `dotnet build`: 0 avisos e 0 erros.
- `dotnet test`: 43 aprovados, 0 falhas.
- EF Core: nenhuma mudança pendente no modelo.
