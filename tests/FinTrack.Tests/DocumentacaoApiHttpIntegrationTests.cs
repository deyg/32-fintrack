using System.Net;
using System.Text.Json;
using Xunit;

namespace FinTrack.Tests;

public sealed class DocumentacaoApiHttpIntegrationTests
{
    [Fact]
    public async Task OpenApiV1EstaDisponivelNoAmbienteDeDesenvolvimento()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/openapi/v1.json");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var documento = JsonDocument.Parse(await response.Content.ReadAsStreamAsync());
        Assert.True(documento.RootElement.TryGetProperty("openapi", out _));
    }

    [Fact]
    public async Task ScalarEstaDisponivelNoAmbienteDeDesenvolvimento()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/scalar");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("text/html", response.Content.Headers.ContentType?.MediaType);
    }
}
