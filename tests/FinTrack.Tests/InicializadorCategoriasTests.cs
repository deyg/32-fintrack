using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Enums;
using FinTrack.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinTrack.Tests;

public sealed class InicializadorCategoriasTests
{
    [Fact]
    public async Task CriaOnzeCategoriasEReaplicacaoEhIdempotente()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<FinTrackDbContext>().UseSqlite(connection).Options;
        await using var db = new FinTrackDbContext(options);
        await db.Database.MigrateAsync();
        var inicializador = new InicializadorCategorias(db);

        Assert.Equal(11, await inicializador.ExecutarAsync(default));
        Assert.Equal(0, await inicializador.ExecutarAsync(default));
        Assert.Equal(11, await db.Categorias.CountAsync());
        Assert.Equal(8, await db.Categorias.CountAsync(x => x.Natureza == NaturezaCategoria.Despesa));
        Assert.Equal(3, await db.Categorias.CountAsync(x => x.Natureza == NaturezaCategoria.Receita));
    }

    [Fact]
    public async Task PreservaCategoriaExistenteESeuIdentificador()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<FinTrackDbContext>().UseSqlite(connection).Options;
        await using var db = new FinTrackDbContext(options);
        await db.Database.MigrateAsync();
        var existente = Categoria.Criar("Moradia", NaturezaCategoria.Despesa).Value;
        db.Categorias.Add(existente);
        await db.SaveChangesAsync();

        var adicionadas = await new InicializadorCategorias(db).ExecutarAsync(default);

        Assert.Equal(10, adicionadas);
        Assert.Equal(existente.Id, (await db.Categorias.SingleAsync(x => x.Nome == "Moradia")).Id);
    }
}
