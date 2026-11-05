using FinTrack.Core.Domain.Enums;
using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using FinTrack.Core.Domain.Results;

namespace FinTrack.Core.Application.UseCases.Categorias.ListarCategorias;

public sealed record ListarCategoriasQuery(
    NaturezaCategoria? Natureza,
    bool? Ativa,
    int Pagina = 1,
    int TamanhoPagina = 20);

public sealed record PaginaCategoriasOutput(
    IReadOnlyList<CategoriaOutput> Itens,
    int Pagina,
    int TamanhoPagina,
    int Total);

public sealed class ListarCategoriasUseCase(IRepositorioCategorias repositorio)
{
    public async Task<Result<PaginaCategoriasOutput>> ExecutarAsync(
        ListarCategoriasQuery query,
        CancellationToken cancellationToken)
    {
        if (query.Pagina < 1 || query.TamanhoPagina is < 1 or > 100)
        {
            return Result<PaginaCategoriasOutput>.Failure(new Error(
                "CATEGORIA_PAGINACAO_INVALIDA",
                "Página deve ser maior que zero e tamanho da página deve estar entre 1 e 100.",
                ErrorType.Validation));
        }

        var (itens, total) = await repositorio.ListarAsync(
            query.Natureza, query.Ativa, query.Pagina, query.TamanhoPagina, cancellationToken);

        return Result<PaginaCategoriasOutput>.Success(new PaginaCategoriasOutput(
            itens.Select(CategoriaOutput.De).ToArray(),
            query.Pagina,
            query.TamanhoPagina,
            total));
    }
}
