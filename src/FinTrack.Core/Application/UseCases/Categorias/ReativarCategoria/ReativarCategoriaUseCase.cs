using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using FinTrack.Core.Domain.Results;
using FinTrack.Core.Application.UseCases.Categorias;

namespace FinTrack.Core.Application.UseCases.Categorias.ReativarCategoria;

public sealed class ReativarCategoriaUseCase(IRepositorioCategorias repositorio)
{
    public async Task<Result<CategoriaOutput>> ExecutarAsync(Guid id, CancellationToken cancellationToken)
    {
        var categoria = await repositorio.ObterPorIdAsync(id, cancellationToken);
        if (categoria is null) return Result<CategoriaOutput>.Failure(new Error(
            "CATEGORIA_NAO_ENCONTRADA", "Categoria não encontrada.", ErrorType.NotFound));
        categoria.Reativar();
        await repositorio.TentarAtualizarAsync(categoria, cancellationToken);
        return Result<CategoriaOutput>.Success(CategoriaOutput.De(categoria));
    }
}
