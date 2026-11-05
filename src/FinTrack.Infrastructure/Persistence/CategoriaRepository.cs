using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Enums;
using FinTrack.Core.Domain.Ports;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence;

public sealed class CategoriaRepository(FinTrackDbContext dbContext) : IRepositorioCategorias
{
    public async Task<bool> TentarAdicionarAsync(Categoria categoria, CancellationToken cancellationToken)
    {
        dbContext.Categorias.Add(categoria);
        return await SalvarAsync(cancellationToken);
    }

    public Task<Categoria?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken) =>
        dbContext.Categorias.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<(IReadOnlyList<Categoria> Itens, int Total)> ListarAsync(
        NaturezaCategoria? natureza,
        bool? ativa,
        int pagina,
        int tamanhoPagina,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Categorias.AsNoTracking().AsQueryable();
        if (natureza.HasValue)
            query = query.Where(x => x.Natureza == natureza.Value);
        if (ativa.HasValue)
            query = query.Where(x => x.Ativa == ativa.Value);

        var total = await query.CountAsync(cancellationToken);
        var itens = await query
            .OrderBy(x => x.Nome)
            .ThenBy(x => x.Id)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync(cancellationToken);
        return (itens, total);
    }

    public Task<bool> TentarAtualizarAsync(Categoria categoria, CancellationToken cancellationToken) =>
        SalvarAsync(cancellationToken);

    private async Task<bool> SalvarAsync(CancellationToken cancellationToken)
    {
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException exception) when (
            exception.InnerException is SqliteException
            {
                SqliteErrorCode: 19,
                SqliteExtendedErrorCode: 2067
            })
        {
            return false;
        }
    }
}
