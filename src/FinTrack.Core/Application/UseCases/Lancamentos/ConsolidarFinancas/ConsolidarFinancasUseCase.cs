using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using FinTrack.Core.Domain.Results;

namespace FinTrack.Core.Application.UseCases.Lancamentos.ConsolidarFinancas;

public sealed record ConsolidadoOutput(decimal Receitas, decimal Despesas, decimal Resultado,
    DateOnly DataInicial, DateOnly DataFinal, IReadOnlyList<ItemConsolidado> PorConta,
    IReadOnlyList<ItemConsolidado> PorCategoria, IReadOnlyList<ItemConsolidado> PorClassificador);

public sealed class ConsolidarFinancasUseCase(IRepositorioLancamentos repositorio)
{
    public async Task<Result<ConsolidadoOutput>> ExecutarAsync(
        DateOnly inicio, DateOnly fim, CancellationToken cancellationToken)
    {
        if (inicio == default || fim == default || inicio > fim)
            return Result<ConsolidadoOutput>.Failure(new Error(
                "CONSOLIDADO_PERIODO_INVALIDO", "Período inválido.", ErrorType.Validation));
        var x = await repositorio.ConsolidarAsync(inicio, fim, cancellationToken);
        return Result<ConsolidadoOutput>.Success(new(x.Receitas, x.Despesas,
            x.Receitas - x.Despesas, inicio, fim, x.PorConta, x.PorCategoria, x.PorClassificador));
    }
}
