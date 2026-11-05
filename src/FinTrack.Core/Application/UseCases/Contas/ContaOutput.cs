using FinTrack.Core.Domain.Entities;

namespace FinTrack.Core.Application.UseCases.Contas;

public sealed record ContaOutput(
    Guid Id, Guid? CategoriaId, string Nome, string? Classificador, bool Ativa)
{
    public static ContaOutput De(Conta conta) => new(
        conta.Id, conta.CategoriaId, conta.Nome, conta.Classificador?.ToString(), conta.Ativa);
}
