namespace FinTrack.Infrastructure.Persistence;

public sealed class CargaInicial
{
    private CargaInicial() { }

    public CargaInicial(string identificador, DateTimeOffset dataAplicacaoUtc)
    {
        Identificador = identificador;
        DataAplicacaoUtc = dataAplicacaoUtc.ToUniversalTime();
    }

    public string Identificador { get; private set; } = string.Empty;
    public DateTimeOffset DataAplicacaoUtc { get; private set; }
}
