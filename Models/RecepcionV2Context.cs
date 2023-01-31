using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DatosPacientes.Models;

public partial class RecepcionV2Context : DbContext
{
    public RecepcionV2Context()
    {
    }

    public RecepcionV2Context(DbContextOptions<RecepcionV2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<GrupoSanguineo> GrupoSanguineos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=RecepcionV2; Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CI_AS");

        modelBuilder.Entity<GrupoSanguineo>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("Grupo_Sanguineo");

            entity.Property(e => e.Codigo).HasComment("Almacena el Codigo de Grupo Sanguineo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Almacena el Nombre de Grupo Sanguineo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
