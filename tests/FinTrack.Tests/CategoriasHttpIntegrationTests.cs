using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace FinTrack.Tests;

public sealed class CategoriasHttpIntegrationTests
{
    [Fact]
    public async Task FluxoHttpCompletoAtendeContrato()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();

        var post = await client.PostAsJsonAsync("/categorias", new
        {
            nome = "Moradia",
            natureza = "DESPESA"
        });
        Assert.Equal(HttpStatusCode.Created, post.StatusCode);
        var criada = await post.Content.ReadFromJsonAsync<JsonElement>();
        var id = criada.GetProperty("id").GetGuid();
        Assert.True(criada.GetProperty("ativa").GetBoolean());

        var get = await client.GetAsync($"/categorias/{id}");
        Assert.Equal(HttpStatusCode.OK, get.StatusCode);

        var lista = await client.GetFromJsonAsync<JsonElement>("/categorias?natureza=DESPESA&ativa=true");
        Assert.Equal(1, lista.GetProperty("total").GetInt32());
        Assert.Single(lista.GetProperty("itens").EnumerateArray());

        var patch = await client.PatchAsJsonAsync($"/categorias/{id}/nome", new { nome = "Casa" });
        Assert.Equal(HttpStatusCode.OK, patch.StatusCode);
        var renomeada = await patch.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Casa", renomeada.GetProperty("nome").GetString());
    }

    [Fact]
    public async Task ValidacoesConflitoENaoEncontradaSaoMapeados()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();

        Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsJsonAsync(
            "/categorias", new { nome = "", natureza = "OUTRA" })).StatusCode);

        await client.PostAsJsonAsync("/categorias", new { nome = "Salário", natureza = "RECEITA" });
        Assert.Equal(HttpStatusCode.Conflict, (await client.PostAsJsonAsync(
            "/categorias", new { nome = " salário ", natureza = "RECEITA" })).StatusCode);

        Assert.Equal(HttpStatusCode.NotFound,
            (await client.GetAsync($"/categorias/{Guid.NewGuid()}")).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest,
            (await client.GetAsync("/categorias?natureza=OUTRA")).StatusCode);
    }
}
