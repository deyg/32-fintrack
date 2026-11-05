using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Results;

namespace FinTrack.Core.Domain.ValueObjects;

public sealed class NomeCategoria
{
    public const int MaximoCaracteres = 100;

    public string Valor { get; }
    public string Normalizado { get; }

    private NomeCategoria(string valor)
    {
        Valor = valor;
        Normalizado = valor.ToUpperInvariant();
    }

    public static Result<NomeCategoria> Criar(string? nome)
    {
        var valor = nome?.Trim();
        if (string.IsNullOrWhiteSpace(valor) || valor.Length > MaximoCaracteres)
        {
            return Result<NomeCategoria>.Failure(new Error(
                "CATEGORIA_NOME_INVALIDO",
                $"Nome deve ter entre 1 e {MaximoCaracteres} caracteres.",
                ErrorType.Validation));
        }

        return Result<NomeCategoria>.Success(new NomeCategoria(valor));
    }
}
