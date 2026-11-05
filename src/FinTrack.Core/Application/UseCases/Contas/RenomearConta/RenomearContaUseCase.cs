using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using FinTrack.Core.Domain.Results;
using FinTrack.Core.Application.UseCases.Contas;

namespace FinTrack.Core.Application.UseCases.Contas.RenomearConta;

public sealed record RenomearContaCommand(Guid Id, string? Nome);

public sealed class RenomearContaUseCase(IContaRepository repositorio)
{
    public async Task<Result<ContaOutput>> ExecutarAsync(
        RenomearContaCommand command, CancellationToken cancellationToken)
    {
        var conta = await repositorio.ObterPorIdAsync(command.Id, cancellationToken);
        if (conta is null)
            return Result<ContaOutput>.Failure(new Error(
                "CONTA_NAO_ENCONTRADA", "Conta não encontrada.", ErrorType.NotFound));

        var resultado = conta.Renomear(command.Nome);
        if (resultado.IsFailure) return Result<ContaOutput>.Failure(resultado.Error);
        if (!await repositorio.TentarAtualizarAsync(conta, cancellationToken))
            return Result<ContaOutput>.Failure(new Error(
                "CONTA_DUPLICADA", "Já existe uma conta com este nome.", ErrorType.Conflict));
        return Result<ContaOutput>.Success(ContaOutput.De(conta));
    }
}
