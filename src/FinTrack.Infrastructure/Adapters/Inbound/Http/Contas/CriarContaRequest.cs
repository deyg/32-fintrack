namespace FinTrack.Infrastructure.Adapters.Inbound.Http.Contas;

public sealed record CriarContaRequest(Guid CategoriaId, string? Nome, string? Classificador)
{
    public CriarContaRequest(string? nome) : this(Guid.NewGuid(), nome, null) { }
}
