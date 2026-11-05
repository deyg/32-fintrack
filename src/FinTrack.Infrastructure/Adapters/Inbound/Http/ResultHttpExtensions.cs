using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Results;

namespace FinTrack.Infrastructure.Adapters.Inbound.Http;

/// <summary>Tradução única dos erros esperados de aplicação para ProblemDetails HTTP.</summary>
public static class ResultHttpExtensions
{
    public static IResult ToHttpResult<T>(
        this Result<T> resultado,
        Func<T, IResult> sucesso) =>
        resultado.Match(sucesso, ToProblemDetails);

    public static IResult ToProblemDetails(Error erro)
    {
        var (status, titulo) = erro.Type switch
        {
            ErrorType.Validation => (StatusCodes.Status400BadRequest, "Dados inválidos"),
            ErrorType.NotFound => (StatusCodes.Status404NotFound, "Recurso não encontrado"),
            ErrorType.Conflict => (StatusCodes.Status409Conflict, "Conflito"),
            ErrorType.BusinessRule => (StatusCodes.Status422UnprocessableEntity, "Regra de negócio"),
            _ => (StatusCodes.Status500InternalServerError, "Erro interno")
        };

        return Results.Problem(
            statusCode: status,
            title: titulo,
            detail: erro.Message,
            extensions: new Dictionary<string, object?> { ["code"] = erro.Code });
    }
}
