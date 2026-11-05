using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace FinTrack.Tests;

public sealed class CategoriaEstadoHttpIntegrationTests
{
    [Fact]
    public async Task InativaBloqueiaNovaContaEReativaIdempotentemente()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();
        var criada = await client.PostAsJsonAsync("/categorias", new { nome = "Moradia", natureza = "DESPESA" });
        var id = (await criada.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("id").GetGuid();

        Assert.Equal(HttpStatusCode.OK, (await client.PatchAsync($"/categorias/{id}/inativacao", null)).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await client.PatchAsync($"/categorias/{id}/inativacao", null)).StatusCode);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, (await client.PostAsJsonAsync("/contas", new
        {
            categoriaId = id, nome = "Aluguel", classificador = "OF"
        })).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await client.PatchAsync($"/categorias/{id}/ativacao", null)).StatusCode);
        Assert.Equal(HttpStatusCode.Created, (await client.PostAsJsonAsync("/contas", new
        {
            categoriaId = id, nome = "Aluguel", classificador = "OF"
        })).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound,
            (await client.PatchAsync($"/categorias/{Guid.NewGuid()}/inativacao", null)).StatusCode);
    }
}
