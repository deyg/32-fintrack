using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Enums;
using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using FinTrack.Core.Domain.Results;

namespace FinTrack.Core.Application.UseCases.Categorias.CriarCategoria;

public sealed record CriarCategoriaCommand(string? Nome, NaturezaCategoria Natureza);

public sealed class CriarCategoriaUseCase(IRepositorioCategorias repositorio)
{
    public async Task<Result<CategoriaOutput>> ExecutarAsync(
        CriarCategoriaCommand command,
        CancellationToken cancellationToken)
    {
        var categoriaResult = Categoria.Criar(command.Nome, command.Natureza);
        if (categoriaResult.IsFailure)
            return Result<CategoriaOutput>.Failure(categoriaResult.Error);

        if (!await repositorio.TentarAdicionarAsync(categoriaResult.Value, cancellationToken))
        {
            return Result<CategoriaOutput>.Failure(new Error(
                "CATEGORIA_DUPLICADA",
                "Já existe uma categoria com este nome.",
                ErrorType.Conflict));
        }

        return Result<CategoriaOutput>.Success(CategoriaOutput.De(categoriaResult.Value));
    }
}
