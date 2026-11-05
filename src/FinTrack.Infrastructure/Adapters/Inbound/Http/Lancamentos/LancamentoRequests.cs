namespace FinTrack.Infrastructure.Adapters.Inbound.Http.Lancamentos;

public sealed record RegistrarLancamentoRequest(
    Guid ContaId, decimal Valor, DateOnly DataRealizacao, string? Descricao);
public sealed record CancelarLancamentoRequest(string? Motivo);
