using FinTrack.Core.Domain.Enums;

namespace FinTrack.Core.Application.UseCases.Contas.CriarConta;

public sealed record CriarContaCommand(
    Guid CategoriaId,
    string? Nome,
    ClassificadorConta? Classificador);
