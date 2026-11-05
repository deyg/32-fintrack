namespace FinTrack.Infrastructure.Adapters.Inbound.Http.Contas;

public sealed record CriarContaRequest(Guid CategoriaId, string? Nome, string? Classificador);
