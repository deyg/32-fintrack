using FinTrack.Core.Application.UseCases.Contas.CriarConta;
using FluentValidation;
using FinTrack.Core.Domain.Ports;
using FinTrack.Infrastructure.Adapters.Inbound.Http.Contas;
using FinTrack.Infrastructure.ExceptionHandling;
using FinTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FinTrackDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("FinTrack")
        ?? "Data Source=fintrack.db"));
builder.Services.AddScoped<IContaRepository, ContaRepository>();
builder.Services.AddScoped<CriarContaUseCase>();
builder.Services.AddScoped<IValidator<CriarContaRequest>, CriarContaRequestValidator>();
builder.Services.AddExceptionHandler<UnexpectedExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();
app.UseExceptionHandler();

// O esquema deve ser migrado explicitamente durante o deploy ou setup local.
// A API não altera o esquema automaticamente ao iniciar.
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapContasEndpoints();
app.Run();

// Ponto de entrada visível aos testes de integração com WebApplicationFactory.
public partial class Program { }
