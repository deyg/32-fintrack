using FinTrack.Core.Application.UseCases.Categorias.ConsultarCategoria;
using FinTrack.Core.Application.UseCases.Categorias.CriarCategoria;
using FinTrack.Core.Application.UseCases.Categorias.ListarCategorias;
using FinTrack.Core.Application.UseCases.Categorias.RenomearCategoria;
using FinTrack.Core.Domain.Enums;
using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Results;
using FluentValidation;

namespace FinTrack.Infrastructure.Adapters.Inbound.Http.Categorias;

public static class CategoriasEndpoints
{
    public static IEndpointRouteBuilder MapCategoriasEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var grupo = endpoints.MapGroup("/categorias");

        grupo.MapPost("/", async (CriarCategoriaRequest request,
            IValidator<CriarCategoriaRequest> validator,
            CriarCategoriaUseCase useCase,
            CancellationToken cancellationToken) =>
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var natureza = Enum.Parse<NaturezaCategoria>(request.Natureza!, true);
            var result = await useCase.ExecutarAsync(
                new CriarCategoriaCommand(request.Nome, natureza), cancellationToken);
            return result.ToHttpResult(categoria => Results.Created($"/categorias/{categoria.Id}", categoria));
        });

        grupo.MapGet("/{id:guid}", async (Guid id,
            ConsultarCategoriaUseCase useCase,
            CancellationToken cancellationToken) =>
            (await useCase.ExecutarAsync(new ConsultarCategoriaQuery(id), cancellationToken))
                .ToHttpResult(Results.Ok));

        grupo.MapGet("/", async (string? natureza, bool? ativa, int pagina, int tamanhoPagina,
            ListarCategoriasUseCase useCase,
            CancellationToken cancellationToken) =>
        {
            NaturezaCategoria? naturezaFiltro = null;
            if (natureza is not null)
            {
                if (!Enum.TryParse<NaturezaCategoria>(natureza, true, out var valor))
                {
                    return Result<PaginaCategoriasOutput>.Failure(new Error(
                        "CATEGORIA_NATUREZA_INVALIDA",
                        "Natureza deve ser RECEITA ou DESPESA.",
                        ErrorType.Validation)).ToHttpResult(Results.Ok);
                }
                naturezaFiltro = valor;
            }

            var query = new ListarCategoriasQuery(
                naturezaFiltro,
                ativa,
                pagina == 0 ? 1 : pagina,
                tamanhoPagina == 0 ? 20 : tamanhoPagina);
            return (await useCase.ExecutarAsync(query, cancellationToken)).ToHttpResult(Results.Ok);
        });

        grupo.MapPatch("/{id:guid}/nome", async (Guid id,
            RenomearCategoriaRequest request,
            IValidator<RenomearCategoriaRequest> validator,
            RenomearCategoriaUseCase useCase,
            CancellationToken cancellationToken) =>
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var result = await useCase.ExecutarAsync(
                new RenomearCategoriaCommand(id, request.Nome), cancellationToken);
            return result.ToHttpResult(Results.Ok);
        });

        return endpoints;
    }
}
