using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using FinTrack.Core.Domain.Results;

namespace FinTrack.Core.Application.UseCases.Contas.CriarConta;

public sealed class CriarContaUseCase(IContaRepository repository)
{
    public async Task<Result<CriarContaOutput>> ExecuteAsync(
        CriarContaCommand command,
        CancellationToken cancellationToken)
    {
        var contaResult = Conta.Criar(command.Nome);
        if (contaResult.IsFailure)
            return Result<CriarContaOutput>.Failure(contaResult.Error);

        var conta = contaResult.Value;
        if (!await repository.TryAddAsync(conta, cancellationToken))
        {
            return Result<CriarContaOutput>.Failure(new Error(
                "CONTA_DUPLICADA", "Já existe uma conta com este nome.", ErrorType.Conflict));
        }

        return Result<CriarContaOutput>.Success(new CriarContaOutput(conta.Id, conta.Nome));
    }
}
