using FinTrack.Core.Application.UseCases.Contas;
using FinTrack.Core.Domain.Enums;
using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using FinTrack.Core.Domain.Results;

namespace FinTrack.Core.Application.UseCases.Contas.ClassificarContaLegada;

public sealed class ClassificarContaLegadaUseCase(
    IContaRepository contas, IRepositorioCategorias categorias, IRepositorioLancamentos lancamentos)
{
    public async Task<Result<ContaOutput>> ExecutarAsync(Guid id, Guid categoriaId,
        ClassificadorConta? classificador, CancellationToken cancellationToken)
    {
        var conta = await contas.ObterPorIdAsync(id, cancellationToken);
        if (conta is null) return Result<ContaOutput>.Failure(new Error(
            "CONTA_NAO_ENCONTRADA", "Conta não encontrada.", ErrorType.NotFound));
        var categoria = await categorias.ObterPorIdAsync(categoriaId, cancellationToken);
        if (categoria?.Ativa != true) return Result<ContaOutput>.Failure(new Error(
            "CATEGORIA_INAPTA", "Categoria ativa não encontrada.", ErrorType.BusinessRule));
        if (await lancamentos.ExisteParaContaAsync(id, cancellationToken))
            return Result<ContaOutput>.Failure(new Error("CONTA_POSSUI_HISTORICO",
                "Conta com histórico não pode ser reclassificada.", ErrorType.BusinessRule));
        var result = conta.ClassificarLegada(categoria.Id, categoria.Natureza, classificador);
        if (result.IsFailure) return Result<ContaOutput>.Failure(result.Error);
        await contas.TentarAtualizarAsync(conta, cancellationToken);
        return Result<ContaOutput>.Success(ContaOutput.De(conta));
    }
}
