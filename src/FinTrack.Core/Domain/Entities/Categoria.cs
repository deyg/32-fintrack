using FinTrack.Core.Domain.Enums;
using FinTrack.Core.Domain.Results;
using FinTrack.Core.Domain.ValueObjects;

namespace FinTrack.Core.Domain.Entities;

public sealed class Categoria
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string NomeNormalizado { get; private set; } = string.Empty;
    public NaturezaCategoria Natureza { get; private set; }
    public bool Ativa { get; private set; }

    private Categoria() { }

    public static Result<Categoria> Criar(string? nome, NaturezaCategoria natureza)
    {
        var nomeResult = NomeCategoria.Criar(nome);
        if (nomeResult.IsFailure)
            return Result<Categoria>.Failure(nomeResult.Error);

        return Result<Categoria>.Success(new Categoria
        {
            Id = Guid.NewGuid(),
            Nome = nomeResult.Value.Valor,
            NomeNormalizado = nomeResult.Value.Normalizado,
            Natureza = natureza,
            Ativa = true
        });
    }

    public Result Renomear(string? nome)
    {
        var nomeResult = NomeCategoria.Criar(nome);
        if (nomeResult.IsFailure)
            return Result.Failure(nomeResult.Error);

        Nome = nomeResult.Value.Valor;
        NomeNormalizado = nomeResult.Value.Normalizado;
        return Result.Success();
    }

    public void Inativar() => Ativa = false;

    public void Reativar() => Ativa = true;
}
