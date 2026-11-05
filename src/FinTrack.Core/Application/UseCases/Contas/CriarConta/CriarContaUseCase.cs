using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using FinTrack.Core.Domain.Results;

namespace FinTrack.Core.Application.UseCases.Contas.CriarConta;

public sealed class CriarContaUseCase(
    IContaRepository repository,
    IRepositorioCategorias repositorioCategorias)
{
    public async Task<Result<CriarContaOutput>> ExecuteAsync(
        CriarContaCommand command,
        CancellationToken cancellationToken)
    {
        var categoria = await repositorioCategorias.ObterPorIdAsync(
            command.CategoriaId, cancellationToken);
        if (categoria is null)
        {
            return Result<CriarContaOutput>.Failure(new Error(
                "CATEGORIA_NAO_ENCONTRADA", "Categoria não encontrada.", ErrorType.NotFound));
        }

        if (!categoria.Ativa)
        {
            return Result<CriarContaOutput>.Failure(new Error(
                "CATEGORIA_INATIVA", "Categoria inativa não admite novas Contas.", ErrorType.BusinessRule));
        }

        var contaResult = Conta.Criar(
            categoria.Id, categoria.Natureza, command.Nome, command.Classificador);
        if (contaResult.IsFailure)
            return Result<CriarContaOutput>.Failure(contaResult.Error);

        var conta = contaResult.Value;
        if (!await repository.TryAddAsync(conta, cancellationToken))
        {
            return Result<CriarContaOutput>.Failure(new Error(
                "CONTA_DUPLICADA", "Já existe uma conta com este nome.", ErrorType.Conflict));
        }

        return Result<CriarContaOutput>.Success(new CriarContaOutput(
            conta.Id,
            conta.CategoriaId!.Value,
            conta.Nome,
            conta.Classificador?.ToString(),
            conta.Ativa));
    }
}
