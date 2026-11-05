# FinTrack Web

SPA Angular do FinTrack para operar Categorias, Contas, Lançamentos e Consolidados.

## Pré-requisitos

- Node.js 22.12 ou superior dentro da linha 22 (a máquina atual usa 22.19).
- API FinTrack em `http://127.0.0.1:5100`, com migrations aplicadas.

## Desenvolvimento

```bash
npm install
npm start
```

Abra `http://127.0.0.1:4200/`. O proxy local encaminha as chamadas HTTP para a API.

## Build

```bash
npm run build
```

O bundle de produção é gravado em `dist/fintrack-web`.

## Testes

```bash
npm test -- --watch=false
```

As regras de negócio permanecem na API; o frontend apenas apresenta contratos e erros HTTP.
