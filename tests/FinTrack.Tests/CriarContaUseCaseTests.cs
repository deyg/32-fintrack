using FinTrack.Core.Application.UseCases.Contas.CriarConta;
using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Ports;
using Xunit;

namespace FinTrack.Tests;

public sealed class CriarContaUseCaseTests
{
    [Fact]
    public async Task RejeitaNomeComMaisDeCemCaracteres()
    {
        var repository = new FakeContaRepository();
        var useCase = new CriarContaUseCase(repository);

        var result = await useCase.ExecuteAsync(
            new CriarContaCommand(new string('A', 101)), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
        Assert.False(repository.FoiChamado);
    }

    [Fact]
    public async Task RepositorioSinalizaDuplicidadeEUseCaseRetornaConflict()
    {
        var repository = new FakeContaRepository { DeveInserir = false };
        var useCase = new CriarContaUseCase(repository);

        var result = await useCase.ExecuteAsync(
            new CriarContaCommand("Conta Corrente"), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
    }

    [Fact]
    public async Task PropagaCancellationTokenAoRepositorio()
    {
        var repository = new FakeContaRepository();
        var useCase = new CriarContaUseCase(repository);
        using var source = new CancellationTokenSource();

        var result = await useCase.ExecuteAsync(
            new CriarContaCommand("Conta Corrente"), source.Token);

        Assert.True(result.IsSuccess);
        Assert.Equal(source.Token, repository.TokenRecebido);
    }

    private sealed class FakeContaRepository : IContaRepository
    {
        public bool DeveInserir { get; init; } = true;
        public bool FoiChamado { get; private set; }
        public CancellationToken TokenRecebido { get; private set; }

        public Task<bool> TryAddAsync(Conta conta, CancellationToken cancellationToken)
        {
            FoiChamado = true;
            TokenRecebido = cancellationToken;
            return Task.FromResult(DeveInserir);
        }

        public Task<Conta?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken) =>
            Task.FromResult<Conta?>(null);
        public Task<(IReadOnlyList<Conta> Itens, int Total)> ListarAsync(
            Guid? categoriaId, bool? ativa, int pagina, int tamanhoPagina,
            CancellationToken cancellationToken) =>
            Task.FromResult(((IReadOnlyList<Conta>)Array.Empty<Conta>(), 0));
        public Task<bool> TentarAtualizarAsync(Conta conta, CancellationToken cancellationToken) =>
            Task.FromResult(true);
    }
}
