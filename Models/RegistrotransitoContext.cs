using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace MultasTransito2.Models;

public partial class RegistrotransitoContext : DbContext
{
    public RegistrotransitoContext()
    {
    }

    public RegistrotransitoContext(DbContextOptions<RegistrotransitoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ciudadano> Ciudadano { get; set; }

    public virtual DbSet<Multa> Multa { get; set; }

    public virtual DbSet<Multasporciudadanoview> Multasporciudadanoview { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;database=registrotransito;password=root;port=3306", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.0.1-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Ciudadano>(entity =>
        {
            entity.HasKey(e => e.NumeroLicenca).HasName("PRIMARY");

            entity.ToTable("ciudadano");

            entity.Property(e => e.NumeroLicenca)
                .HasMaxLength(9)
                .IsFixedLength();
            entity.Property(e => e.MultasAcargo).HasColumnName("MultasACargo");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Multa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("multa");

            entity.HasIndex(e => e.IdCiudadano, "idCiudadano");

            entity.Property(e => e.IdCiudadano)
                .HasMaxLength(9)
                .IsFixedLength()
                .HasColumnName("idCiudadano");
            entity.Property(e => e.Motivo).HasMaxLength(250);
            entity.Property(e => e.SancionPecuniaria).HasPrecision(10, 2);

            entity.HasOne(d => d.IdCiudadanoNavigation).WithMany(p => p.Multa)
                .HasForeignKey(d => d.IdCiudadano)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("multa_ibfk_1");
        });

        modelBuilder.Entity<Multasporciudadanoview>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("multasporciudadanoview");

            entity.Property(e => e.Motivo).HasMaxLength(250);
            entity.Property(e => e.NombreCiudadano).HasMaxLength(50);
            entity.Property(e => e.NumeroLicenca)
                .HasMaxLength(9)
                .IsFixedLength();
            entity.Property(e => e.SancionPecuniaria).HasPrecision(10, 2);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
