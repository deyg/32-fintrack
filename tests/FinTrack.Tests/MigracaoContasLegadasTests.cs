using FinTrack.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Xunit;

namespace FinTrack.Tests;

public sealed class MigracaoContasLegadasTests
{
    [Fact]
    public async Task PreservaContaLegadaSemInventarCategoria()
    {
        await using var conexao = new SqliteConnection("Data Source=:memory:");
        await conexao.OpenAsync();
        var options = new DbContextOptionsBuilder<FinTrackDbContext>().UseSqlite(conexao).Options;
        await using var db = new FinTrackDbContext(options);
        var migrator = db.GetService<IMigrator>();
        await migrator.MigrateAsync("20261105120623_AddCategorias");
        var id = Guid.NewGuid();
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"INSERT INTO Contas (Id, Nome, NomeNormalizado) VALUES ({id}, {"Legada"}, {"LEGADA"})");

        await migrator.MigrateAsync();
        db.ChangeTracker.Clear();
        var conta = await db.Contas.SingleAsync();
        Assert.Equal(id, conta.Id);
        Assert.Null(conta.CategoriaId);
        Assert.Null(conta.Classificador);
        Assert.True(conta.Ativa);
    }
}
