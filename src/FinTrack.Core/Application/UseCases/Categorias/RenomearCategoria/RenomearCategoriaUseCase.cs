using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using FinTrack.Core.Domain.Results;

namespace FinTrack.Core.Application.UseCases.Categorias.RenomearCategoria;

public sealed record RenomearCategoriaCommand(Guid Id, string? Nome);

public sealed class RenomearCategoriaUseCase(IRepositorioCategorias repositorio)
{
    public async Task<Result<CategoriaOutput>> ExecutarAsync(
        RenomearCategoriaCommand command,
        CancellationToken cancellationToken)
    {
        var categoria = await repositorio.ObterPorIdAsync(command.Id, cancellationToken);
        if (categoria is null)
        {
            return Result<CategoriaOutput>.Failure(new Error(
                "CATEGORIA_NAO_ENCONTRADA", "Categoria não encontrada.", ErrorType.NotFound));
        }

        var renomearResult = categoria.Renomear(command.Nome);
        if (renomearResult.IsFailure)
            return Result<CategoriaOutput>.Failure(renomearResult.Error);

        if (!await repositorio.TentarAtualizarAsync(categoria, cancellationToken))
        {
            return Result<CategoriaOutput>.Failure(new Error(
                "CATEGORIA_DUPLICADA",
                "Já existe uma categoria com este nome.",
                ErrorType.Conflict));
        }

        return Result<CategoriaOutput>.Success(CategoriaOutput.De(categoria));
    }
}
