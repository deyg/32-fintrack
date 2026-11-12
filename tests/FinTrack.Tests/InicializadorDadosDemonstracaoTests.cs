using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Enums;
using FinTrack.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinTrack.Tests;

public sealed class InicializadorDadosDemonstracaoTests
{
    private static readonly DateTimeOffset DataFixa =
        new(2026, 11, 12, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task CriaDadosVinculadosUmaVezEPreservaDataDeAplicacao()
    {
        await using var banco = await BancoDeTeste.CriarAsync();
        await new InicializadorCategorias(banco.Context).ExecutarAsync(default);
        var relogio = new RelogioFixo(DataFixa);
        var inicializador = new InicializadorDadosDemonstracao(banco.Context, relogio);

        Assert.True(await inicializador.ExecutarAsync(default));
        var datasOriginais = await banco.Context.Lancamentos
            .OrderBy(x => x.Descricao)
            .Select(x => x.DataRealizacao)
            .ToListAsync();

        relogio.Agora = DataFixa.AddDays(30);

        Assert.False(await inicializador.ExecutarAsync(default));
        Assert.Equal(11, await banco.Context.Categorias.CountAsync());
        Assert.Equal(6, await banco.Context.Contas.CountAsync());
        Assert.Equal(6, await banco.Context.Lancamentos.CountAsync());
        Assert.Equal(1, await banco.Context.CargasIniciais.CountAsync());
        Assert.Equal(datasOriginais, await banco.Context.Lancamentos
            .OrderBy(x => x.Descricao)
            .Select(x => x.DataRealizacao)
            .ToListAsync());
        Assert.Equal(6, await banco.Context.Contas
            .Join(banco.Context.Categorias,
                conta => conta.CategoriaId,
                categoria => (Guid?)categoria.Id,
                (conta, categoria) => new { conta, categoria })
            .CountAsync());
        Assert.Equal(6, await banco.Context.Lancamentos
            .Join(banco.Context.Contas,
                lancamento => lancamento.ContaId,
                conta => conta.Id,
                (lancamento, conta) => lancamento)
            .CountAsync());
    }

    [Fact]
    public async Task FalhaSemInserirQuandoNomeDeContaJaExiste()
    {
        await using var banco = await BancoDeTeste.CriarAsync();
        await new InicializadorCategorias(banco.Context).ExecutarAsync(default);
        var categoria = await banco.Context.Categorias.SingleAsync(x => x.Nome == "Moradia");
        var contaExistente = Conta.Criar(
            categoria.Id,
            NaturezaCategoria.Despesa,
            " Aluguel ",
            ClassificadorConta.OF).Value;
        banco.Context.Contas.Add(contaExistente);
        await banco.Context.SaveChangesAsync();

        var erro = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new InicializadorDadosDemonstracao(banco.Context, new RelogioFixo(DataFixa))
                .ExecutarAsync(default));

        Assert.Contains("Aluguel", erro.Message);
        Assert.Equal(1, await banco.Context.Contas.CountAsync());
        Assert.Empty(await banco.Context.Lancamentos.ToListAsync());
        Assert.Empty(await banco.Context.CargasIniciais.ToListAsync());
    }

    [Fact]
    public async Task FalhaSemInserirQuandoCategoriaNecessariaEstaInativa()
    {
        await using var banco = await BancoDeTeste.CriarAsync();
        await new InicializadorCategorias(banco.Context).ExecutarAsync(default);
        var categoria = await banco.Context.Categorias.SingleAsync(x => x.Nome == "Moradia");
        categoria.Inativar();
        await banco.Context.SaveChangesAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new InicializadorDadosDemonstracao(banco.Context, new RelogioFixo(DataFixa))
                .ExecutarAsync(default));

        Assert.Empty(await banco.Context.Contas.ToListAsync());
        Assert.Empty(await banco.Context.Lancamentos.ToListAsync());
        Assert.Empty(await banco.Context.CargasIniciais.ToListAsync());
    }

    [Fact]
    public async Task FalhaSemInserirQuandoCategoriaNecessariaNaoExiste()
    {
        await using var banco = await BancoDeTeste.CriarAsync();
        await new InicializadorCategorias(banco.Context).ExecutarAsync(default);
        var categoria = await banco.Context.Categorias.SingleAsync(x => x.Nome == "Moradia");
        banco.Context.Categorias.Remove(categoria);
        await banco.Context.SaveChangesAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new InicializadorDadosDemonstracao(banco.Context, new RelogioFixo(DataFixa))
                .ExecutarAsync(default));

        Assert.Empty(await banco.Context.Contas.ToListAsync());
        Assert.Empty(await banco.Context.Lancamentos.ToListAsync());
        Assert.Empty(await banco.Context.CargasIniciais.ToListAsync());
    }

    [Fact]
    public async Task FalhaSemInserirQuandoNaturezaDaCategoriaEhIncompativel()
    {
        await using var banco = await BancoDeTeste.CriarAsync();
        await new InicializadorCategorias(banco.Context).ExecutarAsync(default);
        await banco.Context.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE categorias SET natureza = {NaturezaCategoria.Receita.ToString()} WHERE nome = {"Moradia"}");
        banco.Context.ChangeTracker.Clear();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new InicializadorDadosDemonstracao(banco.Context, new RelogioFixo(DataFixa))
                .ExecutarAsync(default));

        Assert.Empty(await banco.Context.Contas.ToListAsync());
        Assert.Empty(await banco.Context.Lancamentos.ToListAsync());
        Assert.Empty(await banco.Context.CargasIniciais.ToListAsync());
    }

    [Fact]
    public async Task MarcadorExistenteNaoRecriaRegistrosEditadosOuRemovidos()
    {
        await using var banco = await BancoDeTeste.CriarAsync();
        await new InicializadorCategorias(banco.Context).ExecutarAsync(default);
        var inicializador = new InicializadorDadosDemonstracao(
            banco.Context,
            new RelogioFixo(DataFixa));
        Assert.True(await inicializador.ExecutarAsync(default));

        var aluguel = await banco.Context.Contas.SingleAsync(x => x.Nome == "Aluguel");
        Assert.True(aluguel.Renomear("Minha moradia").IsSuccess);
        var lancamento = await banco.Context.Lancamentos
            .OrderBy(x => x.DataRealizacao)
            .FirstAsync();
        banco.Context.Lancamentos.Remove(lancamento);
        await banco.Context.SaveChangesAsync();

        Assert.False(await inicializador.ExecutarAsync(default));
        Assert.Equal("Minha moradia", (await banco.Context.Contas.SingleAsync(x => x.Id == aluguel.Id)).Nome);
        Assert.Equal(6, await banco.Context.Contas.CountAsync());
        Assert.Equal(5, await banco.Context.Lancamentos.CountAsync());
    }

    [Fact]
    public async Task PreservaContasPreexistentesSemColisao()
    {
        await using var banco = await BancoDeTeste.CriarAsync();
        await new InicializadorCategorias(banco.Context).ExecutarAsync(default);
        var categoria = await banco.Context.Categorias.SingleAsync(x => x.Nome == "Rendimentos");
        var contaExistente = Conta.Criar(
            categoria.Id,
            NaturezaCategoria.Receita,
            "Juros de investimentos",
            null).Value;
        banco.Context.Contas.Add(contaExistente);
        await banco.Context.SaveChangesAsync();

        Assert.True(await new InicializadorDadosDemonstracao(
            banco.Context,
            new RelogioFixo(DataFixa)).ExecutarAsync(default));

        Assert.Equal(7, await banco.Context.Contas.CountAsync());
        Assert.Equal("Juros de investimentos",
            (await banco.Context.Contas.SingleAsync(x => x.Id == contaExistente.Id)).Nome);
        Assert.Equal(6, await banco.Context.Lancamentos.CountAsync());
    }

    private sealed class BancoDeTeste(SqliteConnection conexao) : IAsyncDisposable
    {
        public FinTrackDbContext Context { get; } = new(
            new DbContextOptionsBuilder<FinTrackDbContext>()
                .UseSqlite(conexao)
                .Options);

        public static async Task<BancoDeTeste> CriarAsync()
        {
            var conexao = new SqliteConnection("Data Source=:memory:");
            await conexao.OpenAsync();
            var banco = new BancoDeTeste(conexao);
            await banco.Context.Database.MigrateAsync();
            return banco;
        }

        public async ValueTask DisposeAsync()
        {
            await Context.DisposeAsync();
            await conexao.DisposeAsync();
        }
    }

    private sealed class RelogioFixo(DateTimeOffset agora) : TimeProvider
    {
        public DateTimeOffset Agora { get; set; } = agora;

        public override DateTimeOffset GetUtcNow() => Agora;
    }
}
