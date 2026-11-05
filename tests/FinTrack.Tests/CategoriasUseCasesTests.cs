using FinTrack.Core.Application.UseCases.Categorias.ConsultarCategoria;
using FinTrack.Core.Application.UseCases.Categorias.CriarCategoria;
using FinTrack.Core.Application.UseCases.Categorias.ListarCategorias;
using FinTrack.Core.Application.UseCases.Categorias.RenomearCategoria;
using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Enums;
using FinTrack.Core.Domain.Ports;
using Xunit;

namespace FinTrack.Tests;

public sealed class CategoriasUseCasesTests
{
    [Fact]
    public async Task FluxoCriarConsultarListarERenomearFunciona()
    {
        var repositorio = new RepositorioCategoriasFake();
        var criar = new CriarCategoriaUseCase(repositorio);
        var consultar = new ConsultarCategoriaUseCase(repositorio);
        var listar = new ListarCategoriasUseCase(repositorio);
        var renomear = new RenomearCategoriaUseCase(repositorio);

        var criada = await criar.ExecutarAsync(
            new CriarCategoriaCommand("Moradia", NaturezaCategoria.Despesa), default);
        Assert.True(criada.IsSuccess);

        var consultada = await consultar.ExecutarAsync(
            new ConsultarCategoriaQuery(criada.Value.Id), default);
        Assert.Equal("Moradia", consultada.Value.Nome);

        var pagina = await listar.ExecutarAsync(
            new ListarCategoriasQuery(NaturezaCategoria.Despesa, true), default);
        Assert.Single(pagina.Value.Itens);

        var renomeada = await renomear.ExecutarAsync(
            new RenomearCategoriaCommand(criada.Value.Id, "Casa"), default);
        Assert.Equal("Casa", renomeada.Value.Nome);
    }

    [Fact]
    public async Task CriarDuplicadaRetornaConflito()
    {
        var repositorio = new RepositorioCategoriasFake();
        var useCase = new CriarCategoriaUseCase(repositorio);
        await useCase.ExecutarAsync(new CriarCategoriaCommand("Moradia", NaturezaCategoria.Despesa), default);

        var result = await useCase.ExecutarAsync(
            new CriarCategoriaCommand(" moradia ", NaturezaCategoria.Despesa), default);

        Assert.True(result.IsFailure);
        Assert.Equal("CATEGORIA_DUPLICADA", result.Error.Code);
    }

    private sealed class RepositorioCategoriasFake : IRepositorioCategorias
    {
        private readonly List<Categoria> _categorias = [];

        public Task<bool> TentarAdicionarAsync(Categoria categoria, CancellationToken cancellationToken)
        {
            if (_categorias.Any(x => x.NomeNormalizado == categoria.NomeNormalizado))
                return Task.FromResult(false);
            _categorias.Add(categoria);
            return Task.FromResult(true);
        }

        public Task<Categoria?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken) =>
            Task.FromResult(_categorias.SingleOrDefault(x => x.Id == id));

        public Task<(IReadOnlyList<Categoria> Itens, int Total)> ListarAsync(
            NaturezaCategoria? natureza, bool? ativa, int pagina, int tamanhoPagina,
            CancellationToken cancellationToken)
        {
            var query = _categorias.AsEnumerable();
            if (natureza.HasValue) query = query.Where(x => x.Natureza == natureza);
            if (ativa.HasValue) query = query.Where(x => x.Ativa == ativa);
            var itens = query.OrderBy(x => x.Nome).ToArray();
            return Task.FromResult(((IReadOnlyList<Categoria>)itens, itens.Length));
        }

        public Task<bool> TentarAtualizarAsync(Categoria categoria, CancellationToken cancellationToken) =>
            Task.FromResult(_categorias.Count(x => x.NomeNormalizado == categoria.NomeNormalizado) == 1);
    }
}
