using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using FinTrack.Core.Domain.Results;
using FinTrack.Core.Application.UseCases.Contas;

namespace FinTrack.Core.Application.UseCases.Contas.ListarContas;

public sealed record ListarContasQuery(Guid? CategoriaId, bool? Ativa, int Pagina = 1, int TamanhoPagina = 20);
public sealed record PaginaContasOutput(IReadOnlyList<ContaOutput> Itens, int Pagina, int TamanhoPagina, int Total);

public sealed class ListarContasUseCase(IContaRepository repositorio)
{
    public async Task<Result<PaginaContasOutput>> ExecutarAsync(
        ListarContasQuery query, CancellationToken cancellationToken)
    {
        if (query.Pagina < 1 || query.TamanhoPagina is < 1 or > 100)
        {
            return Result<PaginaContasOutput>.Failure(new Error(
                "CONTA_PAGINACAO_INVALIDA", "Paginação inválida.", ErrorType.Validation));
        }

        var (itens, total) = await repositorio.ListarAsync(
            query.CategoriaId, query.Ativa, query.Pagina, query.TamanhoPagina, cancellationToken);
        return Result<PaginaContasOutput>.Success(new(
            itens.Select(ContaOutput.De).ToArray(), query.Pagina, query.TamanhoPagina, total));
    }
}
