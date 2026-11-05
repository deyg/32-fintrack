using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace FinTrack.Tests;

public sealed class ContasHttpIntegrationTests
{
    [Fact]
    public async Task CadastrarContaValidaRetorna201EBody()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();

        using var response = await client.PostAsJsonAsync("/contas", new { nome = "Conta Corrente" });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("Conta Corrente", doc.RootElement.GetProperty("nome").GetString());
        Assert.NotEqual(Guid.Empty, doc.RootElement.GetProperty("id").GetGuid());
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task NomeDuplicadoComVariacaoDeCaixaEEspacosRetorna409()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();

        using var primeiro = await client.PostAsJsonAsync("/contas", new { nome = "Conta Corrente" });
        using var segundo = await client.PostAsJsonAsync("/contas", new { nome = "  cOnTa CoRrEnTe  " });

        Assert.Equal(HttpStatusCode.Created, primeiro.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, segundo.StatusCode);
        using var doc = JsonDocument.Parse(await segundo.Content.ReadAsStringAsync());
        Assert.Equal("CONTA_DUPLICADA", doc.RootElement.GetProperty("code").GetString());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task NomeVazioRetorna400ComErroNoCampo(string nome)
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();

        using var response = await client.PostAsJsonAsync("/contas", new { nome });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.True(doc.RootElement.GetProperty("errors").TryGetProperty("nome", out _));
    }

    [Fact]
    public async Task NomeComMaisDeCemCaracteresRetorna400()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();

        using var response = await client.PostAsJsonAsync("/contas", new { nome = new string('x', 101) });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task NomeComCemCaracteresAposTrimRetorna201()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();

        using var response = await client.PostAsJsonAsync("/contas", new { nome = "  " + new string('x', 100) + "  " });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task NomeAusenteRetorna400()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();
        using var response = await client.PostAsJsonAsync("/contas", new { });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BancoCriadoPorMigrationPossuiHistorico()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();
        using var response = await client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        // A fabrica de testes executa Database.Migrate() antes de criar o cliente.
    }

    [Fact]
    public async Task EndpointHealthRetorna200()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
