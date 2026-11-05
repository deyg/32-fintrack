using FinTrack.Core.Application.UseCases.Lancamentos;
using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using FinTrack.Core.Domain.Results;

namespace FinTrack.Core.Application.UseCases.Lancamentos.CancelarLancamento;

public sealed class CancelarLancamentoUseCase(IRepositorioLancamentos repositorio, TimeProvider relogio)
{
    public async Task<Result<LancamentoOutput>> ExecutarAsync(
        Guid id, string? motivo, CancellationToken cancellationToken)
    {
        var lancamento = await repositorio.ObterPorIdAsync(id, cancellationToken);
        if (lancamento is null) return Result<LancamentoOutput>.Failure(new Error(
            "LANCAMENTO_NAO_ENCONTRADO", "Lançamento não encontrado.", ErrorType.NotFound));
        var result = lancamento.Cancelar(motivo, relogio.GetUtcNow());
        if (result.IsFailure) return Result<LancamentoOutput>.Failure(result.Error);
        await repositorio.SalvarAsync(cancellationToken);
        return Result<LancamentoOutput>.Success(LancamentoOutput.De(lancamento));
    }
}
