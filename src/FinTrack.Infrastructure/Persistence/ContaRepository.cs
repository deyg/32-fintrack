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
}
