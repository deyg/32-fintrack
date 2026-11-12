using FinTrack.Core.Domain.Entities;
using FinTrack.Core.Domain.Enums;
using FinTrack.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence;

public sealed class InicializadorDadosDemonstracao(
    FinTrackDbContext dbContext,
    TimeProvider timeProvider)
{
    private const string IdentificadorCarga = "fintrack-demo-v1";

    private static readonly ContaDemonstracao[] ContasDemonstracao =
    [
        new("Aluguel", "Moradia", NaturezaCategoria.Despesa, ClassificadorConta.OF),
        new("Mercado", "Alimentação", NaturezaCategoria.Despesa, ClassificadorConta.OV),
        new("Energia", "Moradia", NaturezaCategoria.Despesa, ClassificadorConta.OV),
        new("Transporte por aplicativo", "Transporte", NaturezaCategoria.Despesa, ClassificadorConta.NOV),
        new("Salário mensal", "Salário", NaturezaCategoria.Receita, null),
        new("Serviços freelance", "Freelance", NaturezaCategoria.Receita, null)
    ];

    private static readonly LancamentoDemonstracao[] LancamentosDemonstracao =
    [
        new("Aluguel", 1800.00m, 5, "Aluguel"),
        new("Mercado", 620.50m, 4, "Compras do mês"),
        new("Energia", 180.45m, 3, "Conta de energia"),
        new("Transporte por aplicativo", 42.90m, 2, "Corridas"),
        new("Salário mensal", 5000.00m, 1, "Recebimento mensal"),
        new("Serviços freelance", 900.00m, 0, "Trabalho freelance")
    ];

    public async Task<bool> ExecutarAsync(CancellationToken cancellationToken)
    {
        await using var transacao = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        if (await dbContext.CargasIniciais.AnyAsync(
                x => x.Identificador == IdentificadorCarga,
                cancellationToken))
        {
            await transacao.CommitAsync(cancellationToken);
            return false;
        }

        var categorias = await ObterCategoriasAsync(cancellationToken);
        var contasNormalizadas = ContasDemonstracao
            .Select(conta => (Conta: conta, Nome: CriarNomeConta(conta.Nome)))
            .ToArray();
        var nomesNormalizados = contasNormalizadas.Select(x => x.Nome.Normalizado).ToArray();
        var contasEmConflito = await dbContext.Contas
            .Where(x => nomesNormalizados.Contains(x.NomeNormalizado))
            .Select(x => x.Nome)
            .ToListAsync(cancellationToken);

        if (contasEmConflito.Count > 0)
        {
            throw new InvalidOperationException(
                $"A carga {IdentificadorCarga} encontrou Contas existentes com nomes reservados: " +
                $"{string.Join(", ", contasEmConflito)}.");
        }

        var agoraUtc = timeProvider.GetUtcNow();
        var dataBase = DateOnly.FromDateTime(agoraUtc.UtcDateTime);
        var contasCriadas = new Dictionary<string, Conta>(StringComparer.Ordinal);

        foreach (var item in contasNormalizadas)
        {
            var categoria = ObterCategoria(categorias, item.Conta.Categoria, item.Conta.Natureza);
            var resultado = Conta.Criar(
                categoria.Id,
                categoria.Natureza,
                item.Conta.Nome,
                item.Conta.Classificador);

            if (resultado.IsFailure)
            {
                throw new InvalidOperationException(
                    $"Não foi possível criar a Conta de demonstração '{item.Conta.Nome}': " +
                    resultado.Error.Message);
            }

            dbContext.Contas.Add(resultado.Value);
            contasCriadas.Add(item.Nome.Normalizado, resultado.Value);
        }

        foreach (var item in LancamentosDemonstracao)
        {
            var contaNormalizada = NomeConta.Criar(item.NomeConta);
            if (contaNormalizada.IsFailure)
            {
                throw new InvalidOperationException(
                    $"Nome inválido na carga de demonstração: '{item.NomeConta}'.");
            }

            var conta = contasCriadas[contaNormalizada.Value.Normalizado];
            var resultado = Lancamento.Registrar(
                conta.Id,
                item.Valor,
                dataBase.AddDays(-item.DiasAntes),
                item.Descricao,
                dataBase);

            if (resultado.IsFailure)
            {
                throw new InvalidOperationException(
                    $"Não foi possível criar o Lançamento de demonstração '{item.Descricao}': " +
                    resultado.Error.Message);
            }

            dbContext.Lancamentos.Add(resultado.Value);
        }

        dbContext.CargasIniciais.Add(new CargaInicial(IdentificadorCarga, agoraUtc));
        await dbContext.SaveChangesAsync(cancellationToken);
        await transacao.CommitAsync(cancellationToken);
        return true;
    }

    private async Task<IReadOnlyDictionary<string, Categoria>> ObterCategoriasAsync(
        CancellationToken cancellationToken)
    {
        var categorias = await dbContext.Categorias.ToListAsync(cancellationToken);
        return categorias.ToDictionary(x => x.NomeNormalizado, StringComparer.Ordinal);
    }

    private static Categoria ObterCategoria(
        IReadOnlyDictionary<string, Categoria> categorias,
        string nome,
        NaturezaCategoria naturezaEsperada)
    {
        var nomeCategoria = NomeCategoria.Criar(nome);
        if (nomeCategoria.IsFailure ||
            !categorias.TryGetValue(nomeCategoria.Value.Normalizado, out var categoria))
        {
            throw new InvalidOperationException(
                $"A Categoria necessária '{nome}' não existe para a carga de demonstração.");
        }

        if (!categoria.Ativa || categoria.Natureza != naturezaEsperada)
        {
            throw new InvalidOperationException(
                $"A Categoria '{nome}' deve estar ativa e ter natureza {naturezaEsperada}.");
        }

        return categoria;
    }

    private static NomeConta CriarNomeConta(string nome)
    {
        var resultado = NomeConta.Criar(nome);
        if (resultado.IsFailure)
        {
            throw new InvalidOperationException(
                $"Nome inválido na carga de demonstração: '{nome}'.");
        }

        return resultado.Value;
    }

    private sealed record ContaDemonstracao(
        string Nome,
        string Categoria,
        NaturezaCategoria Natureza,
        ClassificadorConta? Classificador);

    private sealed record LancamentoDemonstracao(
        string NomeConta,
        decimal Valor,
        int DiasAntes,
        string Descricao);
}
