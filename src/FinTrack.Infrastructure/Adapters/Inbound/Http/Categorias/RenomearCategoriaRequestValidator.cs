using FinTrack.Core.Domain.ValueObjects;
using FluentValidation;

namespace FinTrack.Infrastructure.Adapters.Inbound.Http.Categorias;

public sealed class RenomearCategoriaRequestValidator : AbstractValidator<RenomearCategoriaRequest>
{
    public RenomearCategoriaRequestValidator()
    {
        RuleFor(x => x.Nome)
            .Custom((nome, context) =>
            {
                var result = NomeCategoria.Criar(nome);
                if (result.IsFailure)
                    context.AddFailure("nome", result.Error.Message);
            });
    }
}
