using FinTrack.Core.Domain.Enums;
using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Results;

namespace FinTrack.Core.Domain.Entities;

public sealed class Lancamento
{
    public Guid Id { get; private set; }
    public Guid ContaId { get; private set; }
    public decimal Valor { get; private set; }
    public DateOnly DataRealizacao { get; private set; }
    public string? Descricao { get; private set; }
    public SituacaoLancamento Situacao { get; private set; }
    public DateTimeOffset? DataCancelamento { get; private set; }
    public string? MotivoCancelamento { get; private set; }

    private Lancamento() { }

    public static Result<Lancamento> Registrar(
        Guid contaId, decimal valor, DateOnly dataRealizacao, string? descricao, DateOnly hoje)
    {
        if (contaId == Guid.Empty || valor <= 0 || decimal.Round(valor, 2) != valor)
            return Result<Lancamento>.Failure(new Error("LANCAMENTO_DADOS_INVALIDOS",
                "Conta é obrigatória e valor deve ser positivo com até duas casas decimais.", ErrorType.Validation));
        if (dataRealizacao == default || dataRealizacao > hoje)
            return Result<Lancamento>.Failure(new Error("LANCAMENTO_DATA_INVALIDA",
                "Data de realização é obrigatória e não pode ser futura.", ErrorType.Validation));
        var texto = descricao?.Trim();
        if (texto?.Length > 200)
            return Result<Lancamento>.Failure(new Error("LANCAMENTO_DESCRICAO_INVALIDA",
                "Descrição deve ter no máximo 200 caracteres.", ErrorType.Validation));
        return Result<Lancamento>.Success(new Lancamento
        {
            Id = Guid.NewGuid(), ContaId = contaId, Valor = valor,
            DataRealizacao = dataRealizacao, Descricao = string.IsNullOrEmpty(texto) ? null : texto,
            Situacao = SituacaoLancamento.Realizado
        });
    }

    public Result Cancelar(string? motivo, DateTimeOffset agora)
    {
        if (Situacao == SituacaoLancamento.Cancelado) return Result.Success();
        var texto = motivo?.Trim();
        if (texto?.Length > 200)
            return Result.Failure(new Error("LANCAMENTO_MOTIVO_INVALIDO",
                "Motivo deve ter no máximo 200 caracteres.", ErrorType.Validation));
        Situacao = SituacaoLancamento.Cancelado;
        DataCancelamento = agora.ToUniversalTime();
        MotivoCancelamento = string.IsNullOrEmpty(texto) ? null : texto;
        return Result.Success();
    }
}
