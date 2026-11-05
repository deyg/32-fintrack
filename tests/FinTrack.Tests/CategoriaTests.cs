using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Enums;
using Xunit;

namespace FinTrack.Tests;

public sealed class CategoriaTests
{
    [Fact]
    public void CriarNormalizaNomeENasceAtiva()
    {
        var result = Categoria.Criar("  Moradia  ", NaturezaCategoria.Despesa);

        Assert.True(result.IsSuccess);
        Assert.Equal("Moradia", result.Value.Nome);
        Assert.Equal("MORADIA", result.Value.NomeNormalizado);
        Assert.Equal(NaturezaCategoria.Despesa, result.Value.Natureza);
        Assert.True(result.Value.Ativa);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CriarRecusaNomeVazio(string? nome)
    {
        var result = Categoria.Criar(nome, NaturezaCategoria.Receita);

        Assert.True(result.IsFailure);
        Assert.Equal("CATEGORIA_NOME_INVALIDO", result.Error.Code);
    }

    [Fact]
    public void RenomearPreservaIdentidadeENatureza()
    {
        var categoria = Categoria.Criar("Salário", NaturezaCategoria.Receita).Value;
        var id = categoria.Id;

        var result = categoria.Renomear("Rendimentos");

        Assert.True(result.IsSuccess);
        Assert.Equal(id, categoria.Id);
        Assert.Equal(NaturezaCategoria.Receita, categoria.Natureza);
        Assert.Equal("Rendimentos", categoria.Nome);
    }
}
