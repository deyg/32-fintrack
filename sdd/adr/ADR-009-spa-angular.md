# ADR-009 — SPA Angular para operação do FinTrack

**Status:** Aceita em 2026-11-05.

## Contexto

O FinTrack possui API HTTP completa para Categorias, Contas, Lançamentos e
Consolidados, mas não possui interface gráfica. Foi solicitada uma página única,
em padrão visual escuro, capaz de operar todas as features existentes.

## Decisão

- Criar `src/FinTrack.Web` como aplicação Angular 22 standalone e independente
  dos projetos .NET.
- Usar `HttpClient` em um serviço tipado para isolar o acesso à API.
- Manter uma única página responsiva, com navegação interna entre visão geral,
  categorias, contas e lançamentos.
- Usar CSS próprio e tema escuro, sem biblioteca de componentes adicional.
- No desenvolvimento, encaminhar as rotas da API para
  `http://127.0.0.1:5100` por proxy do Angular; não habilitar CORS amplo na API.
- Exibir erros HTTP e `ProblemDetails` em mensagens operacionais, sem reproduzir
  regras de domínio no frontend.

## Alternativas consideradas

- Servir os arquivos do Angular pela API: adiado para uma decisão de deploy;
  acoplaria o ciclo de build frontend ao backend sem necessidade no ambiente local.
- Adotar Angular Material: rejeitado neste incremento para evitar dependência e
  identidade visual adicionais.
- Criar múltiplas páginas e rotas: rejeitado porque o requisito é uma SPA de
  página única para operação do MVP.

## Consequências e impacto

- `FinTrack.Web` é um adaptador cliente externo e não referencia `Core` nem
  `Infrastructure`.
- A API continua sendo a autoridade de validação e regras de negócio.
- O frontend ganha build e testes próprios via npm.
- A estratégia de hospedagem será definida separadamente quando houver deploy.

