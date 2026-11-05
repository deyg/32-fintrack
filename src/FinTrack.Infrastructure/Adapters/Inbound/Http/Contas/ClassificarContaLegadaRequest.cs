namespace FinTrack.Infrastructure.Adapters.Inbound.Http.Contas;

public sealed record ClassificarContaLegadaRequest(Guid CategoriaId, string? Classificador);
