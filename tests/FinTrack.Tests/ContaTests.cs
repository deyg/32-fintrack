using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Results;
using Xunit;

namespace FinTrack.Tests;

public sealed class ContaTests
{
    [Fact]
    public void NomeDeCemCaracteresEhValido()
    {
        var result = Conta.Criar(new string('A', 100));
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void NomeDeCentoEUmCaracteresEhInvalido()
    {
        var result = Conta.Criar(new string('A', 101));
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void NomeVazioOuEmBrancoEhInvalido(string nome)
    {
        Assert.True(Conta.Criar(nome).IsFailure);
    }

    [Fact]
    public void NomeNormalizadoIgnoraEspacosExternosECaixa()
    {
        var conta = Conta.Criar("  Conta Corrente ").Value;
        Assert.Equal("Conta Corrente", conta.Nome);
        Assert.Equal("CONTA CORRENTE", conta.NomeNormalizado);
    }

    [Fact]
    public void ResultMatchExecutaOEstadoCorreto()
    {
        var success = Result<int>.Success(7);
        var failure = Result<int>.Failure(new Error("E", "erro", ErrorType.Conflict));
        Assert.Equal("7", success.Match(x => x.ToString(), _ => "falha"));
        Assert.Equal("falha", failure.Match(x => x.ToString(), _ => "falha"));
    }
}
