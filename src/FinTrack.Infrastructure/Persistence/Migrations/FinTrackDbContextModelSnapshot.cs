using System;
using FinTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace FinTrack.Infrastructure.Persistence.Migrations;

[DbContext(typeof(FinTrackDbContext))]
public partial class FinTrackDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "10.0.14");

        modelBuilder.Entity("FinTrack.Core.Domain.Entities.Conta", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("TEXT");

            b.Property<string>("Nome")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("TEXT");

            b.Property<string>("NomeNormalizado")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("TEXT");

            b.HasKey("Id");
            b.HasIndex("NomeNormalizado").IsUnique();
            b.ToTable("Contas");
        });
    }
}
