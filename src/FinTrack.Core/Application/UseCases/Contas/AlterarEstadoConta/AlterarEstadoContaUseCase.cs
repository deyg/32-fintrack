using FinTrack.Core.Application.UseCases.Contas;
using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using FinTrack.Core.Domain.Results;

namespace FinTrack.Core.Application.UseCases.Contas.AlterarEstadoConta;

public sealed class AlterarEstadoContaUseCase(
    IContaRepository contas, IRepositorioCategorias categorias)
{
    public async Task<Result<ContaOutput>> ExecutarAsync(
        Guid id, bool ativar, CancellationToken cancellationToken)
    {
        var conta = await contas.ObterPorIdAsync(id, cancellationToken);
        if (conta is null) return Result<ContaOutput>.Failure(new Error(
            "CONTA_NAO_ENCONTRADA", "Conta não encontrada.", ErrorType.NotFound));
        if (ativar)
        {
            var categoria = conta.CategoriaId.HasValue
                ? await categorias.ObterPorIdAsync(conta.CategoriaId.Value, cancellationToken) : null;
            var resultado = conta.Reativar(categoria?.Ativa == true);
            if (resultado.IsFailure) return Result<ContaOutput>.Failure(resultado.Error);
        }
        else conta.Inativar();
        await contas.TentarAtualizarAsync(conta, cancellationToken);
        return Result<ContaOutput>.Success(ContaOutput.De(conta));
    }
}
