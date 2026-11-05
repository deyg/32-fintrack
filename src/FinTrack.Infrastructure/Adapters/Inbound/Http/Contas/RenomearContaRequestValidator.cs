using FinTrack.Core.Domain.ValueObjects;
using FluentValidation;

namespace FinTrack.Infrastructure.Adapters.Inbound.Http.Contas;

public sealed class RenomearContaRequestValidator : AbstractValidator<RenomearContaRequest>
{
    public RenomearContaRequestValidator()
    {
        RuleFor(x => x.Nome).Custom((nome, contexto) =>
        {
            var resultado = NomeConta.Criar(nome);
            if (resultado.IsFailure) contexto.AddFailure("nome", resultado.Error.Message);
        });
    }
}
