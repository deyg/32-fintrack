using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace FinTrack.Tests;

public sealed class LancamentosHttpIntegrationTests
{
    [Fact]
    public async Task RegistraConsultaCancelaEConsolidaSemCancelados()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();
        var receita = await CriarConta(client, "Salário", "RECEITA", "Salário principal", null);
        var despesa = await CriarConta(client, "Moradia", "DESPESA", "Aluguel", "OF");
        var r = await Registrar(client, receita, 5000m);
        var d = await Registrar(client, despesa, 1500m);

        var consolidado = await client.GetFromJsonAsync<JsonElement>(
            "/consolidados?dataInicial=2020-01-01&dataFinal=2020-12-31");
        Assert.Equal(5000m, consolidado.GetProperty("receitas").GetDecimal());
        Assert.Equal(1500m, consolidado.GetProperty("despesas").GetDecimal());
        Assert.Equal(3500m, consolidado.GetProperty("resultado").GetDecimal());

        Assert.Equal(HttpStatusCode.OK, (await client.PostAsJsonAsync(
            $"/lancamentos/{d}/cancelamento", new { motivo = "Duplicado" })).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await client.PostAsJsonAsync(
            $"/lancamentos/{d}/cancelamento", new { motivo = "Ignorado" })).StatusCode);
        consolidado = await client.GetFromJsonAsync<JsonElement>(
            "/consolidados?dataInicial=2020-01-01&dataFinal=2020-12-31");
        Assert.Equal(0m, consolidado.GetProperty("despesas").GetDecimal());
        Assert.Equal(5000m, consolidado.GetProperty("resultado").GetDecimal());

        var lista = await client.GetFromJsonAsync<JsonElement>("/lancamentos?situacao=CANCELADO");
        Assert.Equal(1, lista.GetProperty("total").GetInt32());
        Assert.NotEqual(Guid.Empty, r);
    }

    [Fact]
    public async Task RejeitaValorDataFuturaEContaInativa()
    {
        using var factory = new FinTrackApiFactory();
        using var client = factory.CreateClient();
        var conta = await CriarConta(client, "Moradia", "DESPESA", "Aluguel", "OF");
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsJsonAsync("/lancamentos", new
        { contaId = conta, valor = 0m, dataRealizacao = "2020-01-01", descricao = "x" })).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsJsonAsync("/lancamentos", new
        { contaId = conta, valor = 1.001m, dataRealizacao = "2020-01-01", descricao = "x" })).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsJsonAsync("/lancamentos", new
        { contaId = conta, valor = 1m, dataRealizacao = "2999-01-01", descricao = "x" })).StatusCode);
        await client.PatchAsync($"/contas/{conta}/inativacao", null);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, (await client.PostAsJsonAsync("/lancamentos", new
        { contaId = conta, valor = 1m, dataRealizacao = "2020-01-01", descricao = "x" })).StatusCode);
    }

    private static async Task<Guid> CriarConta(HttpClient client, string categoria, string natureza,
        string conta, string? classificador)
    {
        var cr = await client.PostAsJsonAsync("/categorias", new { nome = categoria, natureza });
        var categoriaId = (await cr.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("id").GetGuid();
        var co = await client.PostAsJsonAsync("/contas", new { categoriaId, nome = conta, classificador });
        return (await co.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("id").GetGuid();
    }

    private static async Task<Guid> Registrar(HttpClient client, Guid contaId, decimal valor)
    {
        var response = await client.PostAsJsonAsync("/lancamentos", new
        { contaId, valor, dataRealizacao = "2020-06-15", descricao = "Teste" });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("id").GetGuid();
    }
}
