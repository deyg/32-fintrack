using FinTrack.Core.Domain.Enums;
using FinTrack.Core.Domain.ValueObjects;
using FluentValidation;

namespace FinTrack.Infrastructure.Adapters.Inbound.Http.Categorias;

public sealed class CriarCategoriaRequestValidator : AbstractValidator<CriarCategoriaRequest>
{
    public CriarCategoriaRequestValidator()
    {
        RuleFor(x => x.Nome)
            .Custom((nome, context) =>
            {
                var result = NomeCategoria.Criar(nome);
                if (result.IsFailure)
                    context.AddFailure("nome", result.Error.Message);
            });

        RuleFor(x => x.Natureza)
            .Must(natureza => Enum.TryParse<NaturezaCategoria>(natureza, true, out _))
            .WithMessage("Natureza deve ser RECEITA ou DESPESA.");
    }
}
