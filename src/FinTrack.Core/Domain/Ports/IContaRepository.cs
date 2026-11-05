using FinTrack.Core.Domain.Entities;

namespace FinTrack.Core.Domain.Ports;

public interface IContaRepository
{
    Task<bool> TryAddAsync(Conta conta, CancellationToken cancellationToken);
}
