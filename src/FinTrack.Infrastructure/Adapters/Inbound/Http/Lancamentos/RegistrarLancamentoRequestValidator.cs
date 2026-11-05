using FluentValidation;

namespace FinTrack.Infrastructure.Adapters.Inbound.Http.Lancamentos;

public sealed class RegistrarLancamentoRequestValidator : AbstractValidator<RegistrarLancamentoRequest>
{
    public RegistrarLancamentoRequestValidator()
    {
        RuleFor(x => x.ContaId).NotEmpty();
        RuleFor(x => x.Valor).GreaterThan(0).Must(x => decimal.Round(x, 2) == x)
            .WithMessage("Valor deve ser positivo com até duas casas decimais.");
        RuleFor(x => x.DataRealizacao).NotEmpty();
        RuleFor(x => x.Descricao).MaximumLength(200);
    }
}
