namespace FinTrack.Core.Application.UseCases.Contas.CriarConta;

public sealed record CriarContaOutput(
    Guid Id,
    Guid CategoriaId,
    string Nome,
    string? Classificador,
    bool Ativa);
