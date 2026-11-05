using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Enums;
using FinTrack.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence;

public sealed class InicializadorCategorias(FinTrackDbContext dbContext)
{
    private static readonly (string Nome, NaturezaCategoria Natureza)[] Catalogo =
    [
        ("Moradia", NaturezaCategoria.Despesa),
        ("Alimentação", NaturezaCategoria.Despesa),
        ("Transporte", NaturezaCategoria.Despesa),
        ("Saúde", NaturezaCategoria.Despesa),
        ("Educação", NaturezaCategoria.Despesa),
        ("Lazer", NaturezaCategoria.Despesa),
        ("Assinaturas", NaturezaCategoria.Despesa),
        ("Impostos", NaturezaCategoria.Despesa),
        ("Salário", NaturezaCategoria.Receita),
        ("Rendimentos", NaturezaCategoria.Receita),
        ("Freelance", NaturezaCategoria.Receita)
    ];

    public async Task<int> ExecutarAsync(CancellationToken cancellationToken)
    {
        await using var transacao = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        var existentes = await dbContext.Categorias
            .Select(x => x.NomeNormalizado)
            .ToHashSetAsync(cancellationToken);

        var adicionadas = 0;
        foreach (var item in Catalogo)
        {
            var nome = NomeCategoria.Criar(item.Nome).Value;
            if (existentes.Contains(nome.Normalizado))
                continue;

            dbContext.Categorias.Add(Categoria.Criar(item.Nome, item.Natureza).Value);
            existentes.Add(nome.Normalizado);
            adicionadas++;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        await transacao.CommitAsync(cancellationToken);
        return adicionadas;
    }
}
