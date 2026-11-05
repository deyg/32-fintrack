using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Enums;

namespace FinTrack.Core.Domain.Ports;

public sealed record FiltroLancamentos(
    DateOnly? DataInicial, DateOnly? DataFinal, Guid? CategoriaId, Guid? ContaId,
    ClassificadorConta? Classificador, SituacaoLancamento? Situacao, int Pagina, int TamanhoPagina);

public sealed record ItemConsolidado(Guid? Id, string Nome, decimal Total);
public sealed record ConsolidadoFinanceiro(decimal Receitas, decimal Despesas,
    IReadOnlyList<ItemConsolidado> PorConta, IReadOnlyList<ItemConsolidado> PorCategoria,
    IReadOnlyList<ItemConsolidado> PorClassificador);

public interface IRepositorioLancamentos
{
    Task AdicionarAsync(Lancamento lancamento, CancellationToken cancellationToken);
    Task<Lancamento?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task SalvarAsync(CancellationToken cancellationToken);
    Task<bool> ExisteParaContaAsync(Guid contaId, CancellationToken cancellationToken);
    Task<(IReadOnlyList<Lancamento> Itens, int Total)> ListarAsync(
        FiltroLancamentos filtro, CancellationToken cancellationToken);
    Task<ConsolidadoFinanceiro> ConsolidarAsync(
        DateOnly dataInicial, DateOnly dataFinal, CancellationToken cancellationToken);
}
