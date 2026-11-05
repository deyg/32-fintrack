using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Enums;
using FinTrack.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinTrack.Tests;

public sealed class CategoriaRepositoryTests
{
    [Fact]
    public async Task PersisteImpedeDuplicidadeEListaOrdenada()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<FinTrackDbContext>().UseSqlite(connection).Options;
        await using var db = new FinTrackDbContext(options);
        await db.Database.MigrateAsync();
        var repositorio = new CategoriaRepository(db);

        Assert.True(await repositorio.TentarAdicionarAsync(
            Categoria.Criar("Moradia", NaturezaCategoria.Despesa).Value, default));
        Assert.False(await repositorio.TentarAdicionarAsync(
            Categoria.Criar(" moradia ", NaturezaCategoria.Despesa).Value, default));

        db.ChangeTracker.Clear();
        var (itens, total) = await repositorio.ListarAsync(
            NaturezaCategoria.Despesa, true, 1, 20, default);
        Assert.Equal(1, total);
        Assert.Equal("Moradia", Assert.Single(itens).Nome);
    }
}
