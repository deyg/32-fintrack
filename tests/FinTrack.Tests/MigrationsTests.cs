using FinTrack.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinTrack.Tests;

public sealed class MigrationsTests
{
    [Fact]
    public async Task MigraBancoVazioEAReaplicacaoEhIdempotente()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<FinTrackDbContext>()
            .UseSqlite(connection)
            .Options;
        await using var db = new FinTrackDbContext(options);

        await db.Database.MigrateAsync();
        await db.Database.MigrateAsync();

        var aplicadas = await db.Database.GetAppliedMigrationsAsync();
        Assert.Equal(new[] { "20261105000100_InitialCreate" }, aplicadas);
        Assert.True(await db.Database.CanConnectAsync());
        Assert.Equal(0, await db.Contas.CountAsync());
    }
}
