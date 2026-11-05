using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace FinTrack.Tests;

public sealed class ClassificadorContaHttpIntegrationTests
{
    [Fact]
    public async Task AlteraSemHistoricoEBloqueiaComLancamento()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();
        var categoria = await client.PostAsJsonAsync("/categorias", new { nome = "Moradia", natureza = "DESPESA" });
        var categoriaId = (await categoria.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("id").GetGuid();
        var conta = await client.PostAsJsonAsync("/contas", new { categoriaId, nome = "Aluguel", classificador = "OF" });
        var contaId = (await conta.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("id").GetGuid();
        Assert.Equal(HttpStatusCode.OK, (await client.PatchAsJsonAsync(
            $"/contas/{contaId}/classificador", new { classificador = "OV" })).StatusCode);
        await client.PostAsJsonAsync("/lancamentos", new
        { contaId, valor = 100m, dataRealizacao = "2020-01-01", descricao = "x" });
        Assert.Equal(HttpStatusCode.UnprocessableEntity, (await client.PatchAsJsonAsync(
            $"/contas/{contaId}/classificador", new { classificador = "OF" })).StatusCode);
    }
}
