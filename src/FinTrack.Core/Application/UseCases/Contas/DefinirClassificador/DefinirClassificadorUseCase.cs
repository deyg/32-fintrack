using FinTrack.Core.Application.UseCases.Contas;
using FinTrack.Core.Domain.Enums;
using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using FinTrack.Core.Domain.Results;

namespace FinTrack.Core.Application.UseCases.Contas.DefinirClassificador;

public sealed class DefinirClassificadorUseCase(
    IContaRepository contas, IRepositorioCategorias categorias, IRepositorioLancamentos lancamentos)
{
    public async Task<Result<ContaOutput>> ExecutarAsync(
        Guid id, ClassificadorConta classificador, CancellationToken cancellationToken)
    {
        var conta = await contas.ObterPorIdAsync(id, cancellationToken);
        if (conta is null) return Result<ContaOutput>.Failure(new Error(
            "CONTA_NAO_ENCONTRADA", "Conta não encontrada.", ErrorType.NotFound));
        if (await lancamentos.ExisteParaContaAsync(id, cancellationToken))
            return Result<ContaOutput>.Failure(new Error("CONTA_POSSUI_HISTORICO",
                "Classificador não pode mudar após existir Lançamento.", ErrorType.BusinessRule));
        var categoria = conta.CategoriaId.HasValue
            ? await categorias.ObterPorIdAsync(conta.CategoriaId.Value, cancellationToken) : null;
        if (categoria is null) return Result<ContaOutput>.Failure(new Error(
            "CONTA_SEM_CATEGORIA", "Conta não classificada.", ErrorType.BusinessRule));
        var result = conta.DefinirClassificador(categoria.Natureza, classificador);
        if (result.IsFailure) return Result<ContaOutput>.Failure(result.Error);
        await contas.TentarAtualizarAsync(conta, cancellationToken);
        return Result<ContaOutput>.Success(ContaOutput.De(conta));
    }
}
