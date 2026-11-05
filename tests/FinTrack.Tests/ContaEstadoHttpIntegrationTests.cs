using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace FinTrack.Tests;

public sealed class ContaEstadoHttpIntegrationTests
{
    [Fact]
    public async Task InativaEReativaSomenteComCategoriaAtiva()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();
        var categoriaResponse = await client.PostAsJsonAsync("/categorias", new { nome = "Moradia", natureza = "DESPESA" });
        var categoriaId = (await categoriaResponse.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("id").GetGuid();
        var contaResponse = await client.PostAsJsonAsync("/contas", new { categoriaId, nome = "Aluguel", classificador = "OF" });
        var contaId = (await contaResponse.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("id").GetGuid();

        Assert.Equal(HttpStatusCode.OK, (await client.PatchAsync($"/contas/{contaId}/inativacao", null)).StatusCode);
        await client.PatchAsync($"/categorias/{categoriaId}/inativacao", null);
        Assert.Equal(HttpStatusCode.UnprocessableEntity,
            (await client.PatchAsync($"/contas/{contaId}/ativacao", null)).StatusCode);
        await client.PatchAsync($"/categorias/{categoriaId}/ativacao", null);
        Assert.Equal(HttpStatusCode.OK, (await client.PatchAsync($"/contas/{contaId}/ativacao", null)).StatusCode);
    }
}
