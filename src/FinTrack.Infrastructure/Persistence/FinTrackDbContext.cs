using FinTrack.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence;

public sealed class FinTrackDbContext(DbContextOptions<FinTrackDbContext> options) : DbContext(options)
{
    public DbSet<Conta> Contas => Set<Conta>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Lancamento> Lancamentos => Set<Lancamento>();
    public DbSet<CargaInicial> CargasIniciais => Set<CargaInicial>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Conta>(entity =>
        {
            entity.ToTable("Contas");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Nome).HasMaxLength(100).IsRequired();
            entity.Property(x => x.NomeNormalizado).HasMaxLength(100).IsRequired();
            entity.Property(x => x.CategoriaId).HasColumnName("categoria_id");
            entity.Property(x => x.Classificador)
                .HasColumnName("classificador").HasConversion<string>().HasMaxLength(3);
            entity.Property(x => x.Ativa).HasColumnName("ativa").IsRequired();
            entity.HasIndex(x => x.NomeNormalizado).IsUnique();
            entity.HasIndex(x => x.CategoriaId);
            entity.HasOne<Categoria>()
                .WithMany()
                .HasForeignKey(x => x.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("categorias");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Nome).HasColumnName("nome").HasMaxLength(100).IsRequired();
            entity.Property(x => x.NomeNormalizado).HasColumnName("nome_normalizado").HasMaxLength(100).IsRequired();
            entity.Property(x => x.Natureza).HasColumnName("natureza").HasConversion<string>().HasMaxLength(10).IsRequired();
            entity.Property(x => x.Ativa).HasColumnName("ativa").IsRequired();
            entity.HasIndex(x => x.NomeNormalizado).IsUnique();
        });

        modelBuilder.Entity<Lancamento>(entity =>
        {
            entity.ToTable("lancamentos");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.ContaId).HasColumnName("conta_id");
            entity.Property(x => x.Valor).HasColumnName("valor").HasPrecision(18, 2);
            entity.Property(x => x.DataRealizacao).HasColumnName("data_realizacao");
            entity.Property(x => x.Descricao).HasColumnName("descricao").HasMaxLength(200);
            entity.Property(x => x.Situacao).HasColumnName("situacao").HasConversion<string>().HasMaxLength(10);
            entity.Property(x => x.DataCancelamento).HasColumnName("data_cancelamento");
            entity.Property(x => x.MotivoCancelamento).HasColumnName("motivo_cancelamento").HasMaxLength(200);
            entity.HasIndex(x => new { x.DataRealizacao, x.Id });
            entity.HasOne<Conta>().WithMany().HasForeignKey(x => x.ContaId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CargaInicial>(entity =>
        {
            entity.ToTable("cargas_iniciais");
            entity.HasKey(x => x.Identificador);
            entity.Property(x => x.Identificador).HasColumnName("identificador").HasMaxLength(100);
            entity.Property(x => x.DataAplicacaoUtc).HasColumnName("data_aplicacao_utc").IsRequired();
        });
    }
}
