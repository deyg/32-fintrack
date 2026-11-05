using FinTrack.Core.Domain.Entities;

namespace FinTrack.Core.Application.UseCases.Categorias;

public sealed record CategoriaOutput(Guid Id, string Nome, string Natureza, bool Ativa)
{
    public static CategoriaOutput De(Categoria categoria) => new(
        categoria.Id,
        categoria.Nome,
        categoria.Natureza.ToString().ToUpperInvariant(),
        categoria.Ativa);
}
