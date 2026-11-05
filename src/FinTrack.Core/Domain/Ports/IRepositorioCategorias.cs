using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Enums;

namespace FinTrack.Core.Domain.Ports;

public interface IRepositorioCategorias
{
    Task<bool> TentarAdicionarAsync(Categoria categoria, CancellationToken cancellationToken);
    Task<Categoria?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<(IReadOnlyList<Categoria> Itens, int Total)> ListarAsync(
        NaturezaCategoria? natureza,
        bool? ativa,
        int pagina,
        int tamanhoPagina,
        CancellationToken cancellationToken);
    Task<bool> TentarAtualizarAsync(Categoria categoria, CancellationToken cancellationToken);
}
