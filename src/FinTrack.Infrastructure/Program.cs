using FinTrack.Core.Application.UseCases.Contas.CriarConta;
using FinTrack.Core.Application.UseCases.Contas.ConsultarConta;
using FinTrack.Core.Application.UseCases.Contas.ListarContas;
using FinTrack.Core.Application.UseCases.Contas.RenomearConta;
using FinTrack.Core.Application.UseCases.Contas.AlterarEstadoConta;
using FinTrack.Core.Application.UseCases.Contas.DefinirClassificador;
using FinTrack.Core.Application.UseCases.Contas.ClassificarContaLegada;
using FinTrack.Core.Application.UseCases.Categorias.ConsultarCategoria;
using FinTrack.Core.Application.UseCases.Categorias.CriarCategoria;
using FinTrack.Core.Application.UseCases.Categorias.ListarCategorias;
using FinTrack.Core.Application.UseCases.Categorias.RenomearCategoria;
using FinTrack.Core.Application.UseCases.Categorias.InativarCategoria;
using FinTrack.Core.Application.UseCases.Categorias.ReativarCategoria;
using FluentValidation;
using FinTrack.Core.Domain.Ports;
using FinTrack.Infrastructure.Adapters.Inbound.Http.Contas;
using FinTrack.Infrastructure.Adapters.Inbound.Http.Categorias;
using FinTrack.Infrastructure.Adapters.Inbound.Http.Lancamentos;
using FinTrack.Core.Application.UseCases.Lancamentos.RegistrarLancamento;
using FinTrack.Core.Application.UseCases.Lancamentos.CancelarLancamento;
using FinTrack.Core.Application.UseCases.Lancamentos.ConsultarLancamentos;
using FinTrack.Core.Application.UseCases.Lancamentos.ConsolidarFinancas;
using FinTrack.Infrastructure.ExceptionHandling;
using FinTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FinTrackDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("FinTrack")
        ?? "Data Source=fintrack.db"));
builder.Services.AddScoped<IContaRepository, ContaRepository>();
builder.Services.AddScoped<IRepositorioCategorias, CategoriaRepository>();
builder.Services.AddScoped<IRepositorioLancamentos, LancamentoRepository>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<InicializadorCategorias>();
builder.Services.AddScoped<CriarContaUseCase>();
builder.Services.AddScoped<ConsultarContaUseCase>();
builder.Services.AddScoped<ListarContasUseCase>();
builder.Services.AddScoped<RenomearContaUseCase>();
builder.Services.AddScoped<AlterarEstadoContaUseCase>();
builder.Services.AddScoped<DefinirClassificadorUseCase>();
builder.Services.AddScoped<ClassificarContaLegadaUseCase>();
builder.Services.AddScoped<RegistrarLancamentoUseCase>();
builder.Services.AddScoped<CancelarLancamentoUseCase>();
builder.Services.AddScoped<ConsultarLancamentosUseCase>();
builder.Services.AddScoped<ConsolidarFinancasUseCase>();
builder.Services.AddScoped<CriarCategoriaUseCase>();
builder.Services.AddScoped<ConsultarCategoriaUseCase>();
builder.Services.AddScoped<ListarCategoriasUseCase>();
builder.Services.AddScoped<RenomearCategoriaUseCase>();
builder.Services.AddScoped<InativarCategoriaUseCase>();
builder.Services.AddScoped<ReativarCategoriaUseCase>();
builder.Services.AddScoped<IValidator<CriarContaRequest>, CriarContaRequestValidator>();
builder.Services.AddScoped<IValidator<RenomearContaRequest>, RenomearContaRequestValidator>();
builder.Services.AddScoped<IValidator<RegistrarLancamentoRequest>, RegistrarLancamentoRequestValidator>();
builder.Services.AddScoped<IValidator<CriarCategoriaRequest>, CriarCategoriaRequestValidator>();
builder.Services.AddScoped<IValidator<RenomearCategoriaRequest>, RenomearCategoriaRequestValidator>();
builder.Services.AddExceptionHandler<UnexpectedExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

var app = builder.Build();
app.UseExceptionHandler();

if (args.Contains("--inicializar-categorias", StringComparer.OrdinalIgnoreCase))
{
    await using var scope = app.Services.CreateAsyncScope();
    var inicializador = scope.ServiceProvider.GetRequiredService<InicializadorCategorias>();
    var adicionadas = await inicializador.ExecutarAsync(app.Lifetime.ApplicationStopping);
    app.Logger.LogInformation("Inicialização concluída: {Quantidade} categorias adicionadas.", adicionadas);
    return;
}

// O esquema deve ser migrado explicitamente durante o deploy ou setup local.
// A API não altera o esquema automaticamente ao iniciar.
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => options
        .WithTitle("FinTrack API")
        .DisableAgent());
}

app.MapContasEndpoints();
app.MapCategoriasEndpoints();
app.MapLancamentosEndpoints();
app.Run();

// Ponto de entrada visível aos testes de integração com WebApplicationFactory.
public partial class Program { }
