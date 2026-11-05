using FinTrack.Core.Application.UseCases.Contas.CriarConta;
using FinTrack.Infrastructure.Adapters.Inbound.Http;
using FluentValidation;
using FinTrack.Core.Domain.Enums;
using FinTrack.Core.Application.UseCases.Contas.ConsultarConta;
using FinTrack.Core.Application.UseCases.Contas.ListarContas;
using FinTrack.Core.Application.UseCases.Contas.RenomearConta;

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

        endpoints.MapGet("/contas/{id:guid}", async (Guid id, ConsultarContaUseCase useCase,
            CancellationToken cancellationToken) =>
            (await useCase.ExecutarAsync(id, cancellationToken)).ToHttpResult(Results.Ok));

        endpoints.MapGet("/contas", async (Guid? categoriaId, bool? ativa, int? pagina,
            int? tamanhoPagina, ListarContasUseCase useCase, CancellationToken cancellationToken) =>
            (await useCase.ExecutarAsync(new ListarContasQuery(
                categoriaId, ativa, pagina ?? 1, tamanhoPagina ?? 20), cancellationToken))
                .ToHttpResult(Results.Ok));

        endpoints.MapPatch("/contas/{id:guid}/nome", async (Guid id, RenomearContaRequest request,
            IValidator<RenomearContaRequest> validator, RenomearContaUseCase useCase,
            CancellationToken cancellationToken) =>
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid) return Results.ValidationProblem(validation.ToDictionary());
            return (await useCase.ExecutarAsync(
                new RenomearContaCommand(id, request.Nome), cancellationToken)).ToHttpResult(Results.Ok);
        });

        return endpoints;
    }
}
