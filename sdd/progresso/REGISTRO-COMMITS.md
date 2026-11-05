# Registro de commits e validações — FinTrack

**Estado inicial:** nenhum commit de código foi criado como parte deste pacote de documentação.

> Preencher somente com SHAs reais observados no Git. Não inventar hashes nem afirmar execução de testes não realizados.

| Passo | Feature | Artefato de código | Requisito/Regra | Comando de validação | Resultado | Commit SHA | Situação |
|---|---|---|---|---|---|---|---|
| — | — | — | — | — | — | — | Aguardando implementação |

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
