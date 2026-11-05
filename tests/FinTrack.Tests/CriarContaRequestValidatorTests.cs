using FinTrack.Infrastructure.Adapters.Inbound.Http.Contas;
using Xunit;

namespace FinTrack.Tests;

public sealed class CriarContaRequestValidatorTests
{
    private readonly CriarContaRequestValidator _validator = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void RejeitaNomeAusenteVazioOuEspacos(string? nome)
    {
        var result = _validator.Validate(new CriarContaRequest(nome));
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "nome");
    }

    [Fact]
    public void RejeitaNomeAcimaDeCemCaracteres()
    {
        var result = _validator.Validate(new CriarContaRequest(new string('A', 101)));
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("100 caracteres"));
    }

    [Fact]
    public void AceitaCemCaracteresAposTrim()
    {
        var result = _validator.Validate(new CriarContaRequest("  " + new string('A', 100) + "  "));
        Assert.True(result.IsValid);
    }

    [Fact]
    public void AceitaNomeValido()
    {
        var result = _validator.Validate(new CriarContaRequest("Conta Corrente"));
        Assert.True(result.IsValid);
    }
}
