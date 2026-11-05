using FinTrack.Core.Domain.Errors;
using FinTrack.Core.Domain.Results;

namespace FinTrack.Core.Domain.ValueObjects;

/// <summary>
/// Única definição das invariantes e da normalização do nome de uma conta.
/// Pode ser utilizada por qualquer adaptador, sem dependência de frameworks externos.
/// </summary>
public sealed class NomeConta
{
    public const int MaximoCaracteres = 100;

    public string Valor { get; }
    public string Normalizado { get; }

    private NomeConta(string valor)
    {
        Valor = valor;
        Normalizado = valor.ToUpperInvariant();
    }

    public static Result<NomeConta> Criar(string? nome)
    {
        var valor = nome?.Trim();
        if (string.IsNullOrWhiteSpace(valor) || valor.Length > MaximoCaracteres)
        {
            return Result<NomeConta>.Failure(new Error(
                "CONTA_NOME_INVALIDO",
                $"Nome deve ter entre 1 e {MaximoCaracteres} caracteres.",
                ErrorType.Validation));
        }

        return Result<NomeConta>.Success(new NomeConta(valor));
    }
}
