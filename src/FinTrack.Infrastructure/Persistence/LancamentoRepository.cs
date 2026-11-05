using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Enums;
using FinTrack.Core.Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence;

public sealed class LancamentoRepository(FinTrackDbContext db) : IRepositorioLancamentos
{
    public async Task AdicionarAsync(Lancamento lancamento, CancellationToken cancellationToken)
    { db.Lancamentos.Add(lancamento); await db.SaveChangesAsync(cancellationToken); }

    public Task<Lancamento?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Lancamentos.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    public async Task SalvarAsync(CancellationToken cancellationToken) =>
        await db.SaveChangesAsync(cancellationToken);
    public Task<bool> ExisteParaContaAsync(Guid contaId, CancellationToken cancellationToken) =>
        db.Lancamentos.AnyAsync(x => x.ContaId == contaId, cancellationToken);

    public async Task<(IReadOnlyList<Lancamento> Itens, int Total)> ListarAsync(
        FiltroLancamentos f, CancellationToken cancellationToken)
    {
        var query = from l in db.Lancamentos.AsNoTracking()
                    join c in db.Contas.AsNoTracking() on l.ContaId equals c.Id
                    where (!f.DataInicial.HasValue || l.DataRealizacao >= f.DataInicial.Value)
                       && (!f.DataFinal.HasValue || l.DataRealizacao <= f.DataFinal.Value)
                       && (!f.ContaId.HasValue || l.ContaId == f.ContaId.Value)
                       && (!f.CategoriaId.HasValue || c.CategoriaId == f.CategoriaId.Value)
                       && (!f.Classificador.HasValue || c.Classificador == f.Classificador.Value)
                       && (!f.Situacao.HasValue || l.Situacao == f.Situacao.Value)
                    select l;
        var total = await query.CountAsync(cancellationToken);
        var itens = await query.OrderByDescending(x => x.DataRealizacao).ThenBy(x => x.Id)
            .Skip((f.Pagina - 1) * f.TamanhoPagina).Take(f.TamanhoPagina).ToListAsync(cancellationToken);
        return (itens, total);
    }

    public async Task<ConsolidadoFinanceiro> ConsolidarAsync(
        DateOnly inicio, DateOnly fim, CancellationToken cancellationToken)
    {
        var dados = await (from l in db.Lancamentos.AsNoTracking()
            join co in db.Contas.AsNoTracking() on l.ContaId equals co.Id
            join ca in db.Categorias.AsNoTracking() on co.CategoriaId equals ca.Id
            where l.Situacao == SituacaoLancamento.Realizado
               && l.DataRealizacao >= inicio && l.DataRealizacao <= fim
            select new { l.Valor, ContaId = co.Id, Conta = co.Nome, co.Classificador,
                CategoriaId = ca.Id, Categoria = ca.Nome, ca.Natureza }).ToListAsync(cancellationToken);
        var receitas = dados.Where(x => x.Natureza == NaturezaCategoria.Receita).Sum(x => x.Valor);
        var despesas = dados.Where(x => x.Natureza == NaturezaCategoria.Despesa).Sum(x => x.Valor);
        static ItemConsolidado Item(Guid? id, string nome, decimal total) => new(id, nome, total);
        return new(receitas, despesas,
            dados.GroupBy(x => new { x.ContaId, x.Conta }).Select(g => Item(g.Key.ContaId, g.Key.Conta, g.Sum(x => x.Valor))).ToArray(),
            dados.GroupBy(x => new { x.CategoriaId, x.Categoria }).Select(g => Item(g.Key.CategoriaId, g.Key.Categoria, g.Sum(x => x.Valor))).ToArray(),
            dados.Where(x => x.Classificador.HasValue).GroupBy(x => x.Classificador!.Value)
                .Select(g => Item(null, g.Key.ToString(), g.Sum(x => x.Valor))).ToArray());
    }
}
