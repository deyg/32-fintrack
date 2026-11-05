using FinTrack.Core.Domain.Entities;

namespace FinTrack.Core.Application.UseCases.Lancamentos;

public sealed record LancamentoOutput(Guid Id, Guid ContaId, decimal Valor,
    DateOnly DataRealizacao, string? Descricao, string Situacao,
    DateTimeOffset? DataCancelamento, string? MotivoCancelamento)
{
    public static LancamentoOutput De(Lancamento x) => new(x.Id, x.ContaId, x.Valor,
        x.DataRealizacao, x.Descricao, x.Situacao.ToString().ToUpperInvariant(),
        x.DataCancelamento, x.MotivoCancelamento);
}
