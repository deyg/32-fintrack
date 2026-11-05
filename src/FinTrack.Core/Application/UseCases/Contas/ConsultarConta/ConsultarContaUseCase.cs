using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using FinTrack.Core.Domain.Results;
using FinTrack.Core.Application.UseCases.Contas;

namespace FinTrack.Core.Application.UseCases.Contas.ConsultarConta;

public sealed class ConsultarContaUseCase(IContaRepository repositorio)
{
    public async Task<Result<ContaOutput>> ExecutarAsync(Guid id, CancellationToken cancellationToken)
    {
        var conta = await repositorio.ObterPorIdAsync(id, cancellationToken);
        return conta is null
            ? Result<ContaOutput>.Failure(new Error(
                "CONTA_NAO_ENCONTRADA", "Conta não encontrada.", ErrorType.NotFound))
            : Result<ContaOutput>.Success(ContaOutput.De(conta));
    }
}
