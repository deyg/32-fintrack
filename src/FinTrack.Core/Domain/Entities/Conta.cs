using FinTrack.Core.Domain.Results;
using FinTrack.Core.Domain.ValueObjects;
using FinTrack.Core.Domain.Enums;
using FinTrack.Core.Domain.Errors;

namespace FinTrack.Core.Domain.Entities;

public sealed class Conta
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string NomeNormalizado { get; private set; } = string.Empty;
    public Guid? CategoriaId { get; private set; }
    public ClassificadorConta? Classificador { get; private set; }
    public bool Ativa { get; private set; }

    private Conta() { } // Materialização pelo EF Core

    public static Result<Conta> Criar(
        Guid categoriaId,
        NaturezaCategoria natureza,
        string? nome,
        ClassificadorConta? classificador)
    {
        if (categoriaId == Guid.Empty)
        {
            return Result<Conta>.Failure(new Error(
                "CONTA_CATEGORIA_INVALIDA", "Categoria é obrigatória.", ErrorType.Validation));
        }

        if (natureza == NaturezaCategoria.Receita && classificador is not null ||
            natureza == NaturezaCategoria.Despesa && classificador is null)
        {
            return Result<Conta>.Failure(new Error(
                "CONTA_CLASSIFICADOR_INVALIDO",
                "Receitas não possuem classificador e despesas exigem OF, OV, NOF ou NOV.",
                ErrorType.BusinessRule));
        }

        var nomeResult = NomeConta.Criar(nome);
        if (nomeResult.IsFailure) return Result<Conta>.Failure(nomeResult.Error);
        return Result<Conta>.Success(new Conta
        {
            Id = Guid.NewGuid(), CategoriaId = categoriaId,
            Nome = nomeResult.Value.Valor, NomeNormalizado = nomeResult.Value.Normalizado,
            Classificador = classificador, Ativa = true
        });
    }

    public Result Renomear(string? nome)
    {
        var nomeResult = NomeConta.Criar(nome);
        if (nomeResult.IsFailure)
            return Result.Failure(nomeResult.Error);

        Nome = nomeResult.Value.Valor;
        NomeNormalizado = nomeResult.Value.Normalizado;
        return Result.Success();
    }

    public void Inativar() => Ativa = false;

    public Result Reativar(bool categoriaAtiva)
    {
        if (CategoriaId is null || !categoriaAtiva)
            return Result.Failure(new Error("CONTA_REATIVACAO_INVALIDA",
                "Conta exige Categoria ativa e classificada.", ErrorType.BusinessRule));
        Ativa = true;
        return Result.Success();
    }

    public Result DefinirClassificador(NaturezaCategoria natureza, ClassificadorConta? classificador)
    {
        if (natureza == NaturezaCategoria.Receita || classificador is null)
            return Result.Failure(new Error("CONTA_CLASSIFICADOR_INVALIDO",
                "Somente Contas de despesa aceitam classificador.", ErrorType.BusinessRule));
        Classificador = classificador;
        return Result.Success();
    }
}
