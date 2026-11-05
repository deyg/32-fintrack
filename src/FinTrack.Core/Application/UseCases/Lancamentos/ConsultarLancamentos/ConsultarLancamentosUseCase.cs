using FinTrack.Core.Application.UseCases.Lancamentos;
using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using FinTrack.Core.Domain.Results;

namespace FinTrack.Core.Application.UseCases.Lancamentos.ConsultarLancamentos;

public sealed record PaginaLancamentosOutput(IReadOnlyList<LancamentoOutput> Itens,
    int Pagina, int TamanhoPagina, int Total);

public sealed class ConsultarLancamentosUseCase(IRepositorioLancamentos repositorio)
{
    public async Task<Result<PaginaLancamentosOutput>> ExecutarAsync(
        FiltroLancamentos filtro, CancellationToken cancellationToken)
    {
        if (filtro.Pagina < 1 || filtro.TamanhoPagina is < 1 or > 100 ||
            filtro.DataInicial > filtro.DataFinal)
            return Result<PaginaLancamentosOutput>.Failure(new Error(
                "LANCAMENTO_FILTRO_INVALIDO", "Filtros ou paginação inválidos.", ErrorType.Validation));
        var (itens, total) = await repositorio.ListarAsync(filtro, cancellationToken);
        return Result<PaginaLancamentosOutput>.Success(new(
            itens.Select(LancamentoOutput.De).ToArray(), filtro.Pagina, filtro.TamanhoPagina, total));
    }
}
