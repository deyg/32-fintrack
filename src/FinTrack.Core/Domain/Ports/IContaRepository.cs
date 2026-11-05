using FinTrack.Core.Domain.Entities;

namespace FinTrack.Core.Domain.Ports;

public interface IContaRepository
{
    Task<bool> TryAddAsync(Conta conta, CancellationToken cancellationToken);
    Task<Conta?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<(IReadOnlyList<Conta> Itens, int Total)> ListarAsync(
        Guid? categoriaId, bool? ativa, int pagina, int tamanhoPagina,
        CancellationToken cancellationToken);
    Task<bool> TentarAtualizarAsync(Conta conta, CancellationToken cancellationToken);
}
