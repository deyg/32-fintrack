using Microsoft.AspNetCore.Diagnostics;

namespace FinTrack.Infrastructure.ExceptionHandling;

public sealed class UnexpectedExceptionHandler(ILogger<UnexpectedExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Deixa o middleware padrão reconhecer a desconexão/cancelamento.
        if (exception is OperationCanceledException &&
            httpContext.RequestAborted.IsCancellationRequested)
            return false;

        logger.LogError(exception, "Erro inesperado ao processar a requisição");
        await Results.Problem(statusCode: 500, title: "Erro interno").ExecuteAsync(httpContext);
        return true;
    }
}
