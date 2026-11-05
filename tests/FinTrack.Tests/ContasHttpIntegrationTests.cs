using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace FinTrack.Tests;

public sealed class ContasHttpIntegrationTests
{
    [Fact]
    public async Task FluxoCompletoDeContaDeDespesaFunciona()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();
        var categoriaId = await CriarCategoriaAsync(client, "Moradia", "DESPESA");
        var post = await client.PostAsJsonAsync("/contas", new
        {
            categoriaId, nome = "Aluguel", classificador = "OF"
        });
        Assert.Equal(HttpStatusCode.Created, post.StatusCode);
        var criada = await post.Content.ReadFromJsonAsync<JsonElement>();
        var id = criada.GetProperty("id").GetGuid();
        Assert.Equal(categoriaId, criada.GetProperty("categoriaId").GetGuid());
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync($"/contas/{id}")).StatusCode);
        var lista = await client.GetFromJsonAsync<JsonElement>($"/contas?categoriaId={categoriaId}");
        Assert.Equal(1, lista.GetProperty("total").GetInt32());
        Assert.Equal(HttpStatusCode.OK, (await client.PatchAsJsonAsync(
            $"/contas/{id}/nome", new { nome = "Aluguel residencial" })).StatusCode);
    }

    [Fact]
    public async Task RegrasDeNaturezaDuplicidadeEExistenciaSaoAplicadas()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();
        var receitaId = await CriarCategoriaAsync(client, "Salário", "RECEITA");
        Assert.Equal(HttpStatusCode.UnprocessableEntity, (await client.PostAsJsonAsync("/contas", new
        {
            categoriaId = receitaId, nome = "Salário principal", classificador = "OF"
        })).StatusCode);
        Assert.Equal(HttpStatusCode.Created, (await client.PostAsJsonAsync("/contas", new
        {
            categoriaId = receitaId, nome = "Salário principal", classificador = (string?)null
        })).StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, (await client.PostAsJsonAsync("/contas", new
        {
            categoriaId = receitaId, nome = " salário PRINCIPAL ", classificador = (string?)null
        })).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.PostAsJsonAsync("/contas", new
        {
            categoriaId = Guid.NewGuid(), nome = "Outra", classificador = (string?)null
        })).StatusCode);
    }

    [Fact]
    public async Task HealthEstaDisponivel()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/health")).StatusCode);
    }

    private static async Task<Guid> CriarCategoriaAsync(HttpClient client, string nome, string natureza)
    {
        var response = await client.PostAsJsonAsync("/categorias", new { nome, natureza });
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("id").GetGuid();
    }
}
