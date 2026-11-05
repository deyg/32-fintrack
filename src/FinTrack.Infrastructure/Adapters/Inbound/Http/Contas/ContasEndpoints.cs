using FinTrack.Core.Application.UseCases.Contas.CriarConta;
using FinTrack.Infrastructure.Adapters.Inbound.Http;
using FluentValidation;
using FinTrack.Core.Domain.Enums;

namespace FinTrack.Infrastructure.Adapters.Inbound.Http.Contas;

public static class ContasEndpoints
{
    public static IEndpointRouteBuilder MapContasEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/contas", async (
            CriarContaRequest request,
            IValidator<CriarContaRequest> validator,
            CriarContaUseCase useCase,
            CancellationToken cancellationToken) =>
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            ClassificadorConta? classificador = request.Classificador is null
                ? null
                : Enum.Parse<ClassificadorConta>(request.Classificador, true);
            var command = new CriarContaCommand(request.CategoriaId, request.Nome, classificador);
            var result = await useCase.ExecuteAsync(command, cancellationToken);

            return result.ToHttpResult(conta => Results.Created(
                $"/contas/{conta.Id}", conta));
        });

        return endpoints;
    }
}
