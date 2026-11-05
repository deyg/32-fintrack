using FinTrack.Core.Domain.Results;
using FinTrack.Core.Domain.ValueObjects;

namespace FinTrack.Core.Domain.Entities;

public sealed class Conta
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string NomeNormalizado { get; private set; } = string.Empty;

    private Conta() { } // Materialização pelo EF Core

    public static Result<Conta> Criar(string? nome)
    {
        var nomeResult = NomeConta.Criar(nome);
        if (nomeResult.IsFailure)
            return Result<Conta>.Failure(nomeResult.Error);

        var nomeValido = nomeResult.Value;
        return Result<Conta>.Success(new Conta
        {
            Id = Guid.NewGuid(),
            Nome = nomeValido.Valor,
            NomeNormalizado = nomeValido.Normalizado
        });
    }
}
