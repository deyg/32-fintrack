using FinTrack.Core.Domain.Entities;
using FinTrack.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinTrack.Tests;

public sealed class ContaRepositoryTests
{
    [Fact]
    public async Task SQLiteRejeitaNomeDuplicadoIndependenteDeCaixa()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<FinTrackDbContext>()
            .UseSqlite(connection)
            .Options;

        await using (var db = new FinTrackDbContext(options))
        {
            await db.Database.MigrateAsync();
        }

        await using (var db = new FinTrackDbContext(options))
        {
            var inserted = await new ContaRepository(db).TryAddAsync(
                Conta.Criar("Conta Corrente").Value, CancellationToken.None);
            Assert.True(inserted);
        }

        await using (var db = new FinTrackDbContext(options))
        {
            var inserted = await new ContaRepository(db).TryAddAsync(
                Conta.Criar("conta corrente").Value, CancellationToken.None);
            Assert.False(inserted);
        }
    }
}
