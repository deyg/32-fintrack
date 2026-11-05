using FinTrack.Core.Application.UseCases.Lancamentos;
using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using FinTrack.Core.Domain.Results;

namespace FinTrack.Core.Application.UseCases.Lancamentos.RegistrarLancamento;

public sealed record RegistrarLancamentoCommand(Guid ContaId, decimal Valor,
    DateOnly DataRealizacao, string? Descricao);

public sealed class RegistrarLancamentoUseCase(
    IContaRepository contas, IRepositorioCategorias categorias,
    IRepositorioLancamentos lancamentos, TimeProvider relogio)
{
    public async Task<Result<LancamentoOutput>> ExecutarAsync(
        RegistrarLancamentoCommand command, CancellationToken cancellationToken)
    {
        var conta = await contas.ObterPorIdAsync(command.ContaId, cancellationToken);
        if (conta is null) return Result<LancamentoOutput>.Failure(new Error(
            "CONTA_NAO_ENCONTRADA", "Conta não encontrada.", ErrorType.NotFound));
        if (!conta.Ativa || !conta.CategoriaId.HasValue) return Result<LancamentoOutput>.Failure(new Error(
            "CONTA_INAPTA", "Conta ativa e classificada é obrigatória.", ErrorType.BusinessRule));
        var categoria = await categorias.ObterPorIdAsync(conta.CategoriaId.Value, cancellationToken);
        if (categoria?.Ativa != true) return Result<LancamentoOutput>.Failure(new Error(
            "CATEGORIA_INATIVA", "Categoria ativa é obrigatória.", ErrorType.BusinessRule));
        var hoje = DateOnly.FromDateTime(relogio.GetLocalNow().DateTime);
        var result = Lancamento.Registrar(command.ContaId, command.Valor,
            command.DataRealizacao, command.Descricao, hoje);
        if (result.IsFailure) return Result<LancamentoOutput>.Failure(result.Error);
        await lancamentos.AdicionarAsync(result.Value, cancellationToken);
        return Result<LancamentoOutput>.Success(LancamentoOutput.De(result.Value));
    }
}
