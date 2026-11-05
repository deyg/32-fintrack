using FinTrack.Core.Application.UseCases.Lancamentos.CancelarLancamento;
using FinTrack.Core.Application.UseCases.Lancamentos.ConsolidarFinancas;
using FinTrack.Core.Application.UseCases.Lancamentos.ConsultarLancamentos;
using FinTrack.Core.Application.UseCases.Lancamentos.RegistrarLancamento;
using FinTrack.Core.Domain.Enums;
using FinTrack.Core.Domain.Ports;
using FinTrack.Infrastructure.Adapters.Inbound.Http;
using FluentValidation;

namespace FinTrack.Infrastructure.Adapters.Inbound.Http.Lancamentos;

public static class LancamentosEndpoints
{
    public static IEndpointRouteBuilder MapLancamentosEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/lancamentos", async (RegistrarLancamentoRequest request,
            IValidator<RegistrarLancamentoRequest> validator, RegistrarLancamentoUseCase useCase,
            CancellationToken cancellationToken) =>
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid) return Results.ValidationProblem(validation.ToDictionary());
            var result = await useCase.ExecutarAsync(new(request.ContaId, request.Valor,
                request.DataRealizacao, request.Descricao), cancellationToken);
            return result.ToHttpResult(x => Results.Created($"/lancamentos/{x.Id}", x));
        });

        endpoints.MapPost("/lancamentos/{id:guid}/cancelamento", async (Guid id,
            CancelarLancamentoRequest? request, CancelarLancamentoUseCase useCase,
            CancellationToken cancellationToken) =>
            (await useCase.ExecutarAsync(id, request?.Motivo, cancellationToken)).ToHttpResult(Results.Ok));

        endpoints.MapGet("/lancamentos", async (DateOnly? dataInicial, DateOnly? dataFinal,
            Guid? categoriaId, Guid? contaId, string? classificador, string? situacao,
            int? pagina, int? tamanhoPagina, ConsultarLancamentosUseCase useCase,
            CancellationToken cancellationToken) =>
        {
            ClassificadorConta? c = Enum.TryParse<ClassificadorConta>(classificador, true, out var cv) ? cv : null;
            SituacaoLancamento? s = Enum.TryParse<SituacaoLancamento>(situacao, true, out var sv) ? sv : null;
            var filtro = new FiltroLancamentos(dataInicial, dataFinal, categoriaId, contaId,
                c, s, pagina ?? 1, tamanhoPagina ?? 20);
            return (await useCase.ExecutarAsync(filtro, cancellationToken)).ToHttpResult(Results.Ok);
        });

        endpoints.MapGet("/consolidados", async (DateOnly dataInicial, DateOnly dataFinal,
            ConsolidarFinancasUseCase useCase, CancellationToken cancellationToken) =>
            (await useCase.ExecutarAsync(dataInicial, dataFinal, cancellationToken)).ToHttpResult(Results.Ok));
        return endpoints;
    }
}
