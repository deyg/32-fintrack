using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using FinTrack.Core.Domain.Results;

namespace FinTrack.Core.Application.UseCases.Categorias.ConsultarCategoria;

public sealed record ConsultarCategoriaQuery(Guid Id);

public sealed class ConsultarCategoriaUseCase(IRepositorioCategorias repositorio)
{
    public async Task<Result<CategoriaOutput>> ExecutarAsync(
        ConsultarCategoriaQuery query,
        CancellationToken cancellationToken)
    {
        var categoria = await repositorio.ObterPorIdAsync(query.Id, cancellationToken);
        return categoria is null
            ? Result<CategoriaOutput>.Failure(new Error(
                "CATEGORIA_NAO_ENCONTRADA", "Categoria não encontrada.", ErrorType.NotFound))
            : Result<CategoriaOutput>.Success(CategoriaOutput.De(categoria));
    }
}
