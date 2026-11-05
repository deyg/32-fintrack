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
            if (classificador is not null && !Enum.TryParse<ClassificadorConta>(classificador, true, out _))
                return Results.ValidationProblem(new Dictionary<string, string[]>
                    { ["classificador"] = ["Classificador inválido."] });
            if (situacao is not null && !Enum.TryParse<SituacaoLancamento>(situacao, true, out _))
                return Results.ValidationProblem(new Dictionary<string, string[]>
                    { ["situacao"] = ["Situação inválida."] });
            ClassificadorConta? c = classificador is null ? null : Enum.Parse<ClassificadorConta>(classificador, true);
            SituacaoLancamento? s = situacao is null ? null : Enum.Parse<SituacaoLancamento>(situacao, true);
            var filtro = new FiltroLancamentos(dataInicial, dataFinal, categoriaId, contaId,
                c, s, pagina ?? 1, tamanhoPagina ?? 20);
            return (await useCase.ExecutarAsync(filtro, cancellationToken)).ToHttpResult(Results.Ok);
        });

        endpoints.MapGet("/consolidados", async (DateOnly dataInicial, DateOnly dataFinal,
            ConsolidarFinancasUseCase useCase, CancellationToken cancellationToken) =>
            (await useCase.ExecutarAsync(dataInicial, dataFinal, cancellationToken)).ToHttpResult(Results.Ok));
        endpoints.MapGet("/consolidados/por-conta", async (DateOnly dataInicial, DateOnly dataFinal,
            ConsolidarFinancasUseCase useCase, CancellationToken cancellationToken) =>
            (await useCase.ExecutarAsync(dataInicial, dataFinal, cancellationToken))
                .ToHttpResult(x => Results.Ok(x.PorConta)));
        endpoints.MapGet("/consolidados/por-categoria", async (DateOnly dataInicial, DateOnly dataFinal,
            ConsolidarFinancasUseCase useCase, CancellationToken cancellationToken) =>
            (await useCase.ExecutarAsync(dataInicial, dataFinal, cancellationToken))
                .ToHttpResult(x => Results.Ok(x.PorCategoria)));
        endpoints.MapGet("/consolidados/por-classificador", async (DateOnly dataInicial, DateOnly dataFinal,
            ConsolidarFinancasUseCase useCase, CancellationToken cancellationToken) =>
            (await useCase.ExecutarAsync(dataInicial, dataFinal, cancellationToken))
                .ToHttpResult(x => Results.Ok(x.PorClassificador)));
        return endpoints;
    }
}
