using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Ports;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence;

public sealed class ContaRepository(FinTrackDbContext dbContext) : IContaRepository
{
    public async Task<bool> TryAddAsync(Conta conta, CancellationToken cancellationToken)
    {
        dbContext.Contas.Add(conta);
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
            // UNIQUE constraint: regra esperada de concorrência/duplicidade.
            return false;
        }
    }

    public Task<Conta?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken) =>
        dbContext.Contas.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<(IReadOnlyList<Conta> Itens, int Total)> ListarAsync(
        Guid? categoriaId, bool? ativa, int pagina, int tamanhoPagina,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Contas.AsNoTracking().AsQueryable();
        if (categoriaId.HasValue) query = query.Where(x => x.CategoriaId == categoriaId);
        if (ativa.HasValue) query = query.Where(x => x.Ativa == ativa);
        var total = await query.CountAsync(cancellationToken);
        var itens = await query.OrderBy(x => x.Nome).ThenBy(x => x.Id)
            .Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina)
            .ToListAsync(cancellationToken);
        return (itens, total);
    }

    public async Task<bool> TentarAtualizarAsync(Conta conta, CancellationToken cancellationToken)
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
