using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Results;
using FinTrack.Core.Domain.Enums;
using Xunit;

namespace FinTrack.Tests;

public sealed class ContaTests
{
    [Fact]
    public void NomeDeCemCaracteresEhValido()
    {
        var result = CriarReceita(new string('A', 100));
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void NomeDeCentoEUmCaracteresEhInvalido()
    {
        var result = CriarReceita(new string('A', 101));
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void NomeVazioOuEmBrancoEhInvalido(string nome)
    {
        Assert.True(CriarReceita(nome).IsFailure);
    }

    [Fact]
    public void NomeNormalizadoIgnoraEspacosExternosECaixa()
    {
        var conta = CriarReceita("  Conta Corrente ").Value;
        Assert.Equal("Conta Corrente", conta.Nome);
        Assert.Equal("CONTA CORRENTE", conta.NomeNormalizado);
    }

    [Fact]
    public void DespesaExigeClassificadorEReceitaRecusaClassificador()
    {
        Assert.True(Conta.Criar(Guid.NewGuid(), NaturezaCategoria.Despesa, "Aluguel", null).IsFailure);
        Assert.True(Conta.Criar(Guid.NewGuid(), NaturezaCategoria.Receita, "Salário", ClassificadorConta.OF).IsFailure);
    }

    [Fact]
    public void ContaNovaNaoPodeSerClassificadaNovamente()
    {
        var conta = CriarReceita("Salário").Value;
        var result = conta.ClassificarLegada(Guid.NewGuid(), NaturezaCategoria.Receita, null);
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
    }

    private static Result<Conta> CriarReceita(string? nome) =>
        Conta.Criar(Guid.NewGuid(), NaturezaCategoria.Receita, nome, null);

    [Fact]
    public void ResultMatchExecutaOEstadoCorreto()
    {
        var success = Result<int>.Success(7);
        var failure = Result<int>.Failure(new Error("E", "erro", ErrorType.Conflict));
        Assert.Equal("7", success.Match(x => x.ToString(), _ => "falha"));
        Assert.Equal("falha", failure.Match(x => x.ToString(), _ => "falha"));
    }
}
