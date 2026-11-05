using FinTrack.Core.Domain.Entities;
using FinTrack.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FinTrack.Core.Domain.Enums;

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
            var categoria = Categoria.Criar("Receitas", NaturezaCategoria.Receita).Value;
            db.Categorias.Add(categoria);
            await db.SaveChangesAsync();
            var inserted = await new ContaRepository(db).TryAddAsync(
                Conta.Criar(categoria.Id, categoria.Natureza, "Conta Corrente", null).Value, CancellationToken.None);
            Assert.True(inserted);
        }

        await using (var db = new FinTrackDbContext(options))
        {
            var categoria = await db.Categorias.SingleAsync();
            var inserted = await new ContaRepository(db).TryAddAsync(
                Conta.Criar(categoria.Id, categoria.Natureza, "conta corrente", null).Value, CancellationToken.None);
            Assert.False(inserted);
        }
    }
}
