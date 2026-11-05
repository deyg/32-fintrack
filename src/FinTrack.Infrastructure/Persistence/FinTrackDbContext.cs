using FinTrack.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence;

public sealed class FinTrackDbContext(DbContextOptions<FinTrackDbContext> options) : DbContext(options)
{
    public DbSet<Conta> Contas => Set<Conta>();
    public DbSet<Categoria> Categorias => Set<Categoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Conta>(entity =>
        {
            entity.ToTable("Contas");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Nome).HasMaxLength(100).IsRequired();
            entity.Property(x => x.NomeNormalizado).HasMaxLength(100).IsRequired();
            entity.HasIndex(x => x.NomeNormalizado).IsUnique();
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
    }
}
