using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Results;
using FinTrack.Infrastructure.Adapters.Inbound.Http;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace FinTrack.Tests;

public sealed class ResultHttpExtensionsTests
{
    [Theory]
    [InlineData(ErrorType.Validation, 400)]
    [InlineData(ErrorType.NotFound, 404)]
    [InlineData(ErrorType.Conflict, 409)]
    [InlineData(ErrorType.BusinessRule, 422)]
    public void FalhaEhMapeadaParaIResultHttp(ErrorType errorType, int expectedStatus)
    {
        var resultado = Result<string>.Failure(new Error("CODIGO", "Mensagem", errorType));
        var http = resultado.ToHttpResult(_ => Results.Ok());
        Assert.IsAssignableFrom<IStatusCodeHttpResult>(http);
        Assert.Equal(expectedStatus, ((IStatusCodeHttpResult)http).StatusCode);
    }

    [Fact]
    public void SucessoMantemRespostaDoEndpoint()
    {
        var resultado = Result<string>.Success("ok");
        var http = resultado.ToHttpResult(_ => Results.Created("/x", new { id = 1 }));
        Assert.Equal(201, ((IStatusCodeHttpResult)http).StatusCode);
    }
}
