using FinTrack.Core.Application.UseCases.Contas.CriarConta;
using FinTrack.Core.Application.UseCases.Categorias.ConsultarCategoria;
using FinTrack.Core.Application.UseCases.Categorias.CriarCategoria;
using FinTrack.Core.Application.UseCases.Categorias.ListarCategorias;
using FinTrack.Core.Application.UseCases.Categorias.RenomearCategoria;
using FluentValidation;
using FinTrack.Core.Domain.Ports;
using FinTrack.Infrastructure.Adapters.Inbound.Http.Contas;
using FinTrack.Infrastructure.Adapters.Inbound.Http.Categorias;
using FinTrack.Infrastructure.ExceptionHandling;
using FinTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FinTrackDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("FinTrack")
        ?? "Data Source=fintrack.db"));
builder.Services.AddScoped<IContaRepository, ContaRepository>();
builder.Services.AddScoped<IRepositorioCategorias, CategoriaRepository>();
builder.Services.AddScoped<CriarContaUseCase>();
builder.Services.AddScoped<CriarCategoriaUseCase>();
builder.Services.AddScoped<ConsultarCategoriaUseCase>();
builder.Services.AddScoped<ListarCategoriasUseCase>();
builder.Services.AddScoped<RenomearCategoriaUseCase>();
builder.Services.AddScoped<IValidator<CriarContaRequest>, CriarContaRequestValidator>();
builder.Services.AddScoped<IValidator<CriarCategoriaRequest>, CriarCategoriaRequestValidator>();
builder.Services.AddScoped<IValidator<RenomearCategoriaRequest>, RenomearCategoriaRequestValidator>();
builder.Services.AddExceptionHandler<UnexpectedExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();
app.UseExceptionHandler();

// O esquema deve ser migrado explicitamente durante o deploy ou setup local.
// A API não altera o esquema automaticamente ao iniciar.
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapContasEndpoints();
app.MapCategoriasEndpoints();
app.Run();

// Ponto de entrada visível aos testes de integração com WebApplicationFactory.
public partial class Program { }
