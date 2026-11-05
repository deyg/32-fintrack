using FinTrack.Core.Domain.ValueObjects;
using FinTrack.Core.Domain.Enums;
using FluentValidation;

namespace FinTrack.Infrastructure.Adapters.Inbound.Http.Contas;

// A borda valida cedo, reutilizando a mesma regra do domínio, sem reescrevê-la.
public sealed class CriarContaRequestValidator : AbstractValidator<CriarContaRequest>
{
    public CriarContaRequestValidator()
    {
        RuleFor(x => x.Nome).Custom((nome, contexto) =>
        {
            var resultado = NomeConta.Criar(nome);
            if (resultado.IsFailure)
                contexto.AddFailure("nome", resultado.Error.Message);
        });
        RuleFor(x => x.CategoriaId).NotEmpty().WithMessage("Categoria é obrigatória.");
        RuleFor(x => x.Classificador)
            .Must(valor => valor is null || Enum.TryParse<ClassificadorConta>(valor, true, out _))
            .WithMessage("Classificador deve ser OF, OV, NOF ou NOV.");
    }
}
