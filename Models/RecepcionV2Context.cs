using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DatosPacientes.Models
{
    public partial class RecepcionV2Context : DbContext
    {
        public RecepcionV2Context()
        {
        }

        public RecepcionV2Context(DbContextOptions<RecepcionV2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<AreaSalud> AreaSaluds { get; set; } = null!;
        public virtual DbSet<Comunidad> Comunidads { get; set; } = null!;
        public virtual DbSet<Departamento> Departamentos { get; set; } = null!;
        public virtual DbSet<Direccion> Direccions { get; set; } = null!;
        public virtual DbSet<Discapacidad> Discapacidads { get; set; } = null!;
        public virtual DbSet<DistritoSalud> DistritoSaluds { get; set; } = null!;
        public virtual DbSet<Escolaridad> Escolaridads { get; set; } = null!;
        public virtual DbSet<EstadoMigratorio> EstadoMigratorios { get; set; } = null!;
        public virtual DbSet<EtniaMaya> EtniaMayas { get; set; } = null!;
        public virtual DbSet<FuenteRegistro> FuenteRegistros { get; set; } = null!;
        public virtual DbSet<GrupoEtnico> GrupoEtnicos { get; set; } = null!;
        public virtual DbSet<GrupoSanguineo> GrupoSanguineos { get; set; } = null!;
        public virtual DbSet<LugarPoblado> LugarPoblados { get; set; } = null!;
        public virtual DbSet<Municipio> Municipios { get; set; } = null!;
        public virtual DbSet<Nacionalidad> Nacionalidads { get; set; } = null!;
        public virtual DbSet<Ocupacion> Ocupacions { get; set; } = null!;
        public virtual DbSet<Paciente> Pacientes { get; set; } = null!;
        public virtual DbSet<Pai> Pais { get; set; } = null!;
        public virtual DbSet<Persona> Personas { get; set; } = null!;
        public virtual DbSet<Religion> Religions { get; set; } = null!;
        public virtual DbSet<ServicioSigsa> ServicioSigsas { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Modern_Spanish_CI_AS");

            modelBuilder.Entity<AreaSalud>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Area_Salud");

                entity.Property(e => e.Codigo).HasComment("Codigo identificador en la tabla");

                entity.Property(e => e.Altitud)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("altitud")
                    .HasComment("coordenada altitud");

                entity.Property(e => e.Departamento).HasComment("Referencia a la tabla de Departamento");

                entity.Property(e => e.DireccionSede)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("direccion_sede")
                    .HasComment("La direccion de la sede ");

                entity.Property(e => e.DirectorArea)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("director_area")
                    .HasComment("persona encargada o coordinador");

                entity.Property(e => e.Estado).HasComment("Estado del area de salud");

                entity.Property(e => e.Idas)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("idas")
                    .HasComment("identificador del area de Salud -sigsa");

                entity.Property(e => e.Latitud)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("latitud")
                    .HasComment("coordenada latitud");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Nombre o descripcion del area de salud");

                entity.Property(e => e.Region).HasComment("Referencia a la tabla de Region");

                entity.Property(e => e.TelefonoSede)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("telefono_sede")
                    .HasComment("telefono de referencia");

                entity.HasOne(d => d.DepartamentoNavigation)
                    .WithMany(p => p.AreaSaluds)
                    .HasForeignKey(d => d.Departamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Area_Salud_Departamento");
            });

            modelBuilder.Entity<Comunidad>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Comunidad");

                entity.Property(e => e.Codigo).HasComment("Almacena identificador único de comunidad");

                entity.Property(e => e.Altitud)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("coordenada Altitud");

                entity.Property(e => e.Categoria)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Categoria de la comunidad");

                entity.Property(e => e.CentroComunitario)
                    .HasColumnName("Centro_Comunitario")
                    .HasComment("referencia a la tabla de Centro_Comunitario");

                entity.Property(e => e.Departamento).HasComment("referencia a la tabla de Departamento");

                entity.Property(e => e.DistanciaKm).HasComment("referencia SIGSA");

                entity.Property(e => e.Estado).HasComment("Estado de la tupla");

                entity.Property(e => e.Idas)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("idas")
                    .HasComment("referencia al codigo de area -SIGSA");

                entity.Property(e => e.Idc)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("idc")
                    .HasComment("codigo identificador del centro comunidad -SIGSA");

                entity.Property(e => e.Idcc)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("idcc")
                    .HasComment("codigo identificador del centro comunitario -SIGSA");

                entity.Property(e => e.Idds)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("idds")
                    .HasComment("Referencia al codigo de distrito -SIGSA");

                entity.Property(e => e.Idts)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("idts")
                    .HasComment("referencia al codigo de Servicio -SIGSA");

                entity.Property(e => e.Latitud)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("coordenada Latitud");

                entity.Property(e => e.LugarPoblado)
                    .HasColumnName("Lugar_Poblado")
                    .HasComment("Hace referencia a la entidad Lugar_Poblado");

                entity.Property(e => e.Municipio).HasComment("referencia a la tabla de Municipio");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Almacena el nombre de comunidad");

                entity.Property(e => e.Poblacion).HasComment("referencia SIGSA");

                entity.Property(e => e.PuntoConvergencia)
                    .HasColumnName("Punto_Convergencia")
                    .HasComment("referencia SIGSA");

                entity.HasOne(d => d.LugarPobladoNavigation)
                    .WithMany(p => p.Comunidads)
                    .HasForeignKey(d => d.LugarPoblado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comunidad_Lugar_Poblado");
            });

            modelBuilder.Entity<Departamento>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Departamento");

                entity.HasIndex(e => new { e.Nombre, e.Pais }, "IX_Departamento")
                    .IsUnique();

                entity.Property(e => e.Codigo).HasComment("Almacena el identificador único de Departamento");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Almacena el nombre del Deparamento");

                entity.Property(e => e.Pais).HasComment("Hace referencia a la entidad Pais");

                entity.HasOne(d => d.PaisNavigation)
                    .WithMany(p => p.Departamentos)
                    .HasForeignKey(d => d.Pais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Departamento_Pais");
            });

            modelBuilder.Entity<Direccion>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Direccion");

                entity.Property(e => e.Codigo).HasComment("Este es el identificador unico de la tabla direccion");

                entity.Property(e => e.CodigoPostal)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Codigo_Postal")
                    .HasComment("Almacena el código postal de la persona");

                entity.Property(e => e.Colonia)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Almacena la colonia donde vive");

                entity.Property(e => e.Comunidad).HasComment("Hace referencia a la entidad Comunidad");

                entity.Property(e => e.Departamento).HasComment("Hace referencia a la entidad Departamento");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Almacena la descripción de la dirección");

                entity.Property(e => e.DireccionCompleta)
                    .IsUnicode(false)
                    .HasColumnName("Direccion_Completa")
                    .HasComment("Almacena la dirección completa en donde vive");

                entity.Property(e => e.LugarPoblado)
                    .HasColumnName("Lugar_Poblado")
                    .HasComment("Hace referencia a la entidad Lugar Poblado");

                entity.Property(e => e.Municipio).HasComment("Hace referencia a la entidad Municipio");

                entity.Property(e => e.Pais).HasComment("Hace referencia a la entidad Pais");

                entity.Property(e => e.Referencia)
                    .IsUnicode(false)
                    .HasComment("Almacena la referencia de donde vive");

                entity.Property(e => e.Zona)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Almacena la zona en donde vive");

                entity.HasOne(d => d.ComunidadNavigation)
                    .WithMany(p => p.Direccions)
                    .HasForeignKey(d => d.Comunidad)
                    .HasConstraintName("FK_Direccion_Comunidad");

                entity.HasOne(d => d.DepartamentoNavigation)
                    .WithMany(p => p.Direccions)
                    .HasForeignKey(d => d.Departamento)
                    .HasConstraintName("FK_Direccion_Departamento");

                entity.HasOne(d => d.LugarPobladoNavigation)
                    .WithMany(p => p.Direccions)
                    .HasForeignKey(d => d.LugarPoblado)
                    .HasConstraintName("FK_Direccion_Lugar_Poblado");

                entity.HasOne(d => d.MunicipioNavigation)
                    .WithMany(p => p.Direccions)
                    .HasForeignKey(d => d.Municipio)
                    .HasConstraintName("FK_Direccion_Municipio");

                entity.HasOne(d => d.PaisNavigation)
                    .WithMany(p => p.Direccions)
                    .HasForeignKey(d => d.Pais)
                    .HasConstraintName("FK_Direccion_Pais");
            });

            modelBuilder.Entity<Discapacidad>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Discapacidad");

                entity.Property(e => e.Codigo).HasComment("Identificador único para una discapacidad");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Contiene el nombre o descripción de la discapacidad");
            });

            modelBuilder.Entity<DistritoSalud>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Distrito_Salud");

                entity.Property(e => e.Codigo).HasComment("Codigo identificador en la tabla");

                entity.Property(e => e.Altitud)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("altitud")
                    .HasComment("coordenada altitud");

                entity.Property(e => e.AreaSalud)
                    .HasColumnName("Area_Salud")
                    .HasComment("Referencia a la tabla de area_salud");

                entity.Property(e => e.Coordinador)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("coordinador")
                    .HasComment("persona encargada o coordinador");

                entity.Property(e => e.Departamento).HasComment("Referencia a la tabla de Departamento");

                entity.Property(e => e.DireccionSede)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("direccion_sede")
                    .HasComment("La direccion de la sede ");

                entity.Property(e => e.Estado).HasComment("Estado del distrito");

                entity.Property(e => e.Idas)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("idas")
                    .HasComment("identificador del area de Salud -sigsa");

                entity.Property(e => e.Idds)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("idds")
                    .HasComment("identificador del Distrito de Salud -sigsa");

                entity.Property(e => e.Latitud)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("latitud")
                    .HasComment("coordenada latitud");

                entity.Property(e => e.Municipio).HasComment("Referencia a la tabla de Municipio");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Nombre o descripcion del distrito");

                entity.Property(e => e.TelefonoSede)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("telefono_sede")
                    .HasComment("telefono de referencia");

                entity.HasOne(d => d.AreaSaludNavigation)
                    .WithMany(p => p.DistritoSaluds)
                    .HasForeignKey(d => d.AreaSalud)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Distrito_Salud_Area_Salud");
            });

            modelBuilder.Entity<Escolaridad>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Escolaridad");

                entity.Property(e => e.Codigo).HasComment("Identificador único de Escolaridad");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Almacena el nombre de escolaridad");
            });

            modelBuilder.Entity<EstadoMigratorio>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Estado_Migratorio");

                entity.Property(e => e.Codigo).HasComment("Identificador único para un Estado Migratorio");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Contiene el nombre o descripción de un Estado Migratorio");
            });

            modelBuilder.Entity<EtniaMaya>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Etnia_Maya");

                entity.Property(e => e.Codigo).HasComment("Almacena el codigo de Etnia_Maya");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Almacena el nombre de la sub_Grupo_Etnico");
            });

            modelBuilder.Entity<FuenteRegistro>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Fuente_Registro");

                entity.Property(e => e.Codigo).HasComment("Almacena el codigo de Fuente de Ingreso");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Almacena el Nombre de Fuente de Ingreso");
            });

            modelBuilder.Entity<GrupoEtnico>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Grupo_Etnico");

                entity.Property(e => e.Codigo).HasComment("Identificador único para el grupo étnico");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Contiene el nombre o descripción del Grupo Étnico");
            });

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

            modelBuilder.Entity<LugarPoblado>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Lugar_Poblado");

                entity.Property(e => e.Codigo).HasComment("Codigo identificador de la tupla");

                entity.Property(e => e.Categoria)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Categoria en que se clasifica el lugar poblado");

                entity.Property(e => e.Categoria2004)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("categoria2004")
                    .HasComment("Categoria en que se clasifica el lugar poblado ine2004");

                entity.Property(e => e.Departamento).HasComment("referencia a la tabla de Municipio");

                entity.Property(e => e.Idlp)
                    .HasColumnName("idlp")
                    .HasComment("referencia al codigo de lugar poblado-SIGSA");

                entity.Property(e => e.Ine2002)
                    .HasColumnName("ine2002")
                    .HasComment("Categoria en que se clasifica el lugar poblado ine2002");

                entity.Property(e => e.Ine2004)
                    .HasColumnName("ine2004")
                    .HasComment("Categoria en que se clasifica el lugar poblado ine2004");

                entity.Property(e => e.Municipio).HasComment("referencia a la tabla de Municipio");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasComment("Nombre o descripcion del lugar_poblado");

                entity.HasOne(d => d.MunicipioNavigation)
                    .WithMany(p => p.LugarPoblados)
                    .HasForeignKey(d => d.Municipio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lugar_Poblado_Municipio");
            });

            modelBuilder.Entity<Municipio>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Municipio");

                entity.HasIndex(e => new { e.Nombre, e.Departamento }, "IX_Municipio")
                    .IsUnique();

                entity.Property(e => e.Codigo).HasComment("Almacena identificador único de Municipio");

                entity.Property(e => e.Cabecera).HasComment("guarda si el municipio es cabecera departamental");

                entity.Property(e => e.Departamento).HasComment("Hace referencia a la entidad Departamento");

                entity.Property(e => e.Idmun)
                    .HasColumnName("idmun")
                    .HasComment("referencia al codigo de Municipio segun SIGSA");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Almacena el nombre del Municipio");

                entity.HasOne(d => d.DepartamentoNavigation)
                    .WithMany(p => p.Municipios)
                    .HasForeignKey(d => d.Departamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Municipio_Departamento");
            });

            modelBuilder.Entity<Nacionalidad>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Nacionalidad");

                entity.HasIndex(e => e.Nombre, "IX_Nacionalidad")
                    .IsUnique();

                entity.Property(e => e.Codigo).HasComment("Almacena identificador único de Nacionalidad");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Almacena la nacionalidad existentes");
            });

            modelBuilder.Entity<Ocupacion>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Ocupacion");

                entity.HasIndex(e => e.Nombre, "IX_Ocupacion")
                    .IsUnique();

                entity.Property(e => e.Codigo).HasComment("Identificador único para una ocupación");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Contiene el nombre o descripción de la ocupación");
            });

            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Paciente");

                entity.Property(e => e.Codigo).HasComment("Almacena el identificador único de paciente");

                entity.Property(e => e.ArchivoFisico)
                    .HasColumnName("Archivo_Fisico")
                    .HasComment("Describe si existe o no archivo físico del paciente");

                entity.Property(e => e.CaracteristicasPacienteXxx)
                    .IsUnicode(false)
                    .HasColumnName("Caracteristicas_Paciente_XXX")
                    .HasComment("Almacena las caracteristicas de un paciente no identificado");

                entity.Property(e => e.Cliente).HasComment("Contiene el nombre de un cliente");

                entity.Property(e => e.DireccionResponsable)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Direccion_Responsable")
                    .HasComment("Almacena la dirección completa del responsable del paciente");

                entity.Property(e => e.Discapacidad).HasComment("Hace referencia a la entidad Discapacidad");

                entity.Property(e => e.DonanteOrganos)
                    .HasColumnName("Donante_Organos")
                    .HasDefaultValueSql("((0))")
                    .HasComment("Almacena si el paciente es donante de Organos");

                entity.Property(e => e.DonanteSangre)
                    .HasColumnName("Donante_Sangre")
                    .HasDefaultValueSql("((0))")
                    .HasComment("Almacena si el paciente es dondante de sangre");

                entity.Property(e => e.Escolaridad).HasComment("Hace referencia a la entidad Escolaridad");

                entity.Property(e => e.EstadoMigratorio)
                    .HasColumnName("Estado_Migratorio")
                    .HasComment("Hace referencia a la entidad Estado_Migratorio");

                entity.Property(e => e.EtniaMaya)
                    .HasColumnName("Etnia_Maya")
                    .HasComment("Almacena la sub división de Grup_Etnico");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Registro")
                    .HasComment("Fecha de Registro del paciente");

                entity.Property(e => e.Fotografia)
                    .HasColumnType("image")
                    .HasComment("Contiene la fotografía del Paciente");

                entity.Property(e => e.FuenteRegistro)
                    .HasColumnName("Fuente_Registro")
                    .HasComment("Fuente de Registro del Paciente");

                entity.Property(e => e.GrupoEtnico)
                    .HasColumnName("Grupo_Etnico")
                    .HasComment("Hace referencia a la entidad Grupo_Etnico");

                entity.Property(e => e.GrupoSanguineo)
                    .HasColumnName("Grupo_Sanguineo")
                    .HasComment("Almacena el Grupo_Sanguineo del paciente");

                entity.Property(e => e.Idas)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("idas")
                    .HasComment("Almacena Datos de Sigsa");

                entity.Property(e => e.Idds)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("idds")
                    .HasComment("Almacena Datos de Sigsa");

                entity.Property(e => e.Idts)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("idts")
                    .HasComment("Almacena Datos de Sigsa");

                entity.Property(e => e.LugarNacimiento)
                    .IsUnicode(false)
                    .HasColumnName("Lugar_Nacimiento")
                    .HasComment("Contiene el lugar de nacimiento del Paciente");

                entity.Property(e => e.LugarTrabajo)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Lugar_Trabajo")
                    .HasComment("Almacena el lugar de trabajo del paciente");

                entity.Property(e => e.Nacionalidad).HasComment("Contiene la Nacionalidad del Paciente, hace referencia a la entidad Nacionalidad");

                entity.Property(e => e.NoAdmision)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("No_Admision")
                    .HasComment("Almacena el Número de admisión de un paciente (obsoleto)");

                entity.Property(e => e.NoHistoriaClinica)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("No_Historia_Clinica")
                    .HasComment("Almacena el numero de registro de historia clínica de un paciente");

                entity.Property(e => e.NoIdentificado)
                    .HasColumnName("No_Identificado")
                    .HasComment("Almacena si el paciente es XXX");

                entity.Property(e => e.NoSeguroSocial)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("No_Seguro_Social")
                    .HasComment("Almacena el número de Seguro Social del Paciente");

                entity.Property(e => e.NombreCompleto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Nombre_Completo")
                    .HasComment("Almacena el nombre completo del paciente");

                entity.Property(e => e.NombreConyuge)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Nombre_Conyuge")
                    .HasComment("Almacena el nombre completo de la esposa o esposo");

                entity.Property(e => e.NombreMadre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Nombre_Madre")
                    .HasComment("Almacena el nombre completo de la madre del paciente");

                entity.Property(e => e.NombrePadre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Nombre_Padre")
                    .HasComment("Almacena el nombre completo del padre del paciente");

                entity.Property(e => e.NombreResponsable)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Nombre_Responsable")
                    .HasComment("Almacena el nombre completo del responsable del paciente");

                entity.Property(e => e.Ocupacion).HasComment("Almacena la ocupacion de un paciente (Agricultor, Amada de Casa, etc.)");

                entity.Property(e => e.OrientacionSexual)
                    .HasColumnName("Orientacion_Sexual")
                    .HasComment("referencia a la tabla de Orientacion sexual");

                entity.Property(e => e.Persona).HasComment("Hacer referencia a la entidad Persona");

                entity.Property(e => e.ProfesionSexual)
                    .HasColumnName("Profesion_Sexual")
                    .HasComment("referencia a la tabla de Profesion sexual");

                entity.Property(e => e.Publicidad).HasComment("Medio por el cual se entero de los servicios prestados por el centro asistencial");

                entity.Property(e => e.Referente)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Institucion o persona que la refirio al centro asistencial");

                entity.Property(e => e.Religion).HasComment("Almacena la religión del paciente (Evangelico, Catalico, Mormon, Ateo, Testigo de Jehova, etc.)");

                entity.Property(e => e.Reo).HasComment("Almacena si un paciente es un reo o no");

                entity.Property(e => e.Rup)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RUP")
                    .HasComment("Almacena el Registro Unico de Paciente");

                entity.Property(e => e.SeguroSocial)
                    .HasColumnName("Seguro_Social")
                    .HasDefaultValueSql("((0))")
                    .HasComment("Almacena si un paciente tiene seguro social o no.");

                entity.Property(e => e.SupervivenciaMadre)
                    .HasColumnName("Supervivencia_Madre")
                    .HasDefaultValueSql("((1))")
                    .HasComment("Almacena el estado de supervivencia de la madre del paciente (Viva o Muerta)");

                entity.Property(e => e.SupervivenciaPaciente)
                    .HasColumnName("Supervivencia_Paciente")
                    .HasComment("hace referencia a si el paciente sale vivo o muerto");

                entity.Property(e => e.SupervivenciaPadre)
                    .HasColumnName("Supervivencia_Padre")
                    .HasDefaultValueSql("((1))")
                    .HasComment("Almacena el estado de supervivencia del padre del paciente (Vivo o Muerto)");

                entity.Property(e => e.TelefonoResponsable)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Telefono_Responsable")
                    .HasComment("Almacena el numero de telefono del responsable del paciente");

                entity.Property(e => e.Template).HasComment("Contiene la huella digital del Paciente");

                entity.HasOne(d => d.DiscapacidadNavigation)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.Discapacidad)
                    .HasConstraintName("FK_Paciente_Discapacidad");

                entity.HasOne(d => d.EscolaridadNavigation)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.Escolaridad)
                    .HasConstraintName("FK_Paciente_Escolaridad");

                entity.HasOne(d => d.EstadoMigratorioNavigation)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.EstadoMigratorio)
                    .HasConstraintName("FK_Paciente_Estado_Migratorio");

                entity.HasOne(d => d.EtniaMayaNavigation)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.EtniaMaya)
                    .HasConstraintName("FK_Paciente_Etnia_Maya");

                entity.HasOne(d => d.FuenteRegistroNavigation)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.FuenteRegistro)
                    .HasConstraintName("FK_Paciente_Fuente_Registro");

                entity.HasOne(d => d.GrupoEtnicoNavigation)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.GrupoEtnico)
                    .HasConstraintName("FK_Paciente_Grupo_Etnico");

                entity.HasOne(d => d.GrupoSanguineoNavigation)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.GrupoSanguineo)
                    .HasConstraintName("FK_Paciente_Grupo_Sanguineo");

                entity.HasOne(d => d.NacionalidadNavigation)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.Nacionalidad)
                    .HasConstraintName("FK_Paciente_Nacionalidad");

                entity.HasOne(d => d.OcupacionNavigation)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.Ocupacion)
                    .HasConstraintName("FK_Paciente_Ocupacion");

                entity.HasOne(d => d.PersonaNavigation)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.Persona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Paciente_Persona");

                entity.HasOne(d => d.ReligionNavigation)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.Religion)
                    .HasConstraintName("FK_Paciente_Religion");
            });

            modelBuilder.Entity<Pai>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.HasIndex(e => e.Nombre, "IX_Pais")
                    .IsUnique();

                entity.Property(e => e.Codigo).HasComment("Almacena el identificador único de Pais");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Almacena el nombre del país");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Persona");

                entity.Property(e => e.Codigo).HasComment("Identificador unico para una persona");

                entity.Property(e => e.Apellido1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((1))")
                    .HasComment("Contiene el primer apellido de la persona")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Apellido2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Contiene el segundo apellido de la persona")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Apellido3)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Contiene el Apellido de casada")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CodigoIxchel)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Codigo_Ixchel")
                    .HasComment("Contiene el  código generado por el sistema para identificar a la persona (obsoleto)")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CodigoRenap)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Codigo_Renap")
                    .HasComment("Contiene el Id de persona")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Direccion).HasComment("Contiene la direccion de la persona individual o juridica o bien de la agencia");

                entity.Property(e => e.EdadAnios)
                    .HasColumnName("Edad_Anios")
                    .HasComment("Contiene la edad de la persona en años");

                entity.Property(e => e.EdadDias)
                    .HasColumnName("Edad_Dias")
                    .HasComment("Contiene la edad de la persona en dias");

                entity.Property(e => e.EdadMeses)
                    .HasColumnName("Edad_Meses")
                    .HasComment("Contiene la edad de la persona en meses");

                entity.Property(e => e.Email)
                    .IsUnicode(false)
                    .HasComment("Contiene el correo electronico de la persona")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Estado)
                    .HasDefaultValueSql("((1))")
                    .HasComment("Contiene el estado en que se encuenta la persona siendo 1 activo, 0 inactivo");

                entity.Property(e => e.EstadoCivil)
                    .HasColumnName("Estado_Civil")
                    .HasDefaultValueSql("((1))")
                    .HasComment("Contiene el estado civil de la persona, ya sea casado o soltero");

                entity.Property(e => e.FechaDefuncion)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Defuncion")
                    .HasComment("Almacena la fecha de muerte de la persona");

                entity.Property(e => e.FechaNacimiento)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Nacimiento")
                    .HasComment("Contiene la fecha de nacimiento de la persona");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Registro")
                    .HasComment("Guarda la fecha de registro de la persona");

                entity.Property(e => e.Movil)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Contiene el numero de telefono movil de la persona")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Nit)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Contiene el numero de identificacion tributaria de la persona")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Nombre1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Contiene el primer nombre de la persona")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Nombre2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Contiene el segundo nombre de la persona")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.NombreCompleto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Nombre_Completo")
                    .HasComment("Contiene el nombre completo de la persona")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Observaciones)
                    .IsUnicode(false)
                    .HasComment("Contiene observaciones sobre la persona")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.PersonaJuridica)
                    .HasColumnName("Persona_Juridica")
                    .HasComment("Determina si es persona individual o juridica");

                entity.Property(e => e.RazonSocial)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Razon_Social")
                    .HasComment("Contiene el nombre de la razon social")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Sexo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasComment("Contiene el sexo de la persona")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Telefono1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Contiene el telefono de la persona")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Telefono2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Contienen el segundo telefono de la persona en caso que el primer telefono no funcionara")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.HasOne(d => d.DireccionNavigation)
                    .WithMany(p => p.Personas)
                    .HasForeignKey(d => d.Direccion)
                    .HasConstraintName("FK_Persona_Direccion");
            });

            modelBuilder.Entity<Religion>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Religion");

                entity.HasIndex(e => e.Nombre, "IX_Religion")
                    .IsUnique();

                entity.Property(e => e.Codigo).HasComment("Identificador único para una religión");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Contiene el nombre o descripción de la religión");
            });

            modelBuilder.Entity<ServicioSigsa>(entity =>
            {
                entity.HasKey(e => e.Codigo)
                    .HasName("PK_Descripcion_Servicio_Sigsa");

                entity.ToTable("Servicio_Sigsa");

                entity.Property(e => e.Codigo).HasComment("Codigo identificador de la tupla");

                entity.Property(e => e.Altitud)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("coordenada altitud");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Direccion fisica del servicio");

                entity.Property(e => e.DistritoSalud)
                    .HasColumnName("Distrito_Salud")
                    .HasComment("referencia a la tabla de Distrito_Salud");

                entity.Property(e => e.Email)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasComment("email del empleado del servico");

                entity.Property(e => e.Empleado).HasComment("codigo de empleado");

                entity.Property(e => e.Estado).HasComment("Determina el estado de la tupla en la tabla eliminado/activo");

                entity.Property(e => e.Idas)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("idas")
                    .HasComment("referencia al codigo de area -SIGSA");

                entity.Property(e => e.Idds)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("idds")
                    .HasComment("Referencia al codigo de distrito -SIGSA");

                entity.Property(e => e.Idts)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("idts")
                    .HasComment("referencia al codigo de Servicio -SIGSA");

                entity.Property(e => e.JurisdiccionMixta)
                    .HasColumnName("Jurisdiccion_Mixta")
                    .HasComment("identificacion de jurisdiccion mixta");

                entity.Property(e => e.Latitud)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("coordenada latitud");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Nombre o descripcion del servicio");

                entity.Property(e => e.Responsable)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Responsable del servicio");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("telefono de contacto del servicio");

                entity.Property(e => e.TipoServicio)
                    .HasColumnName("Tipo_Servicio")
                    .HasComment("referencia a la tabla de Tipo_Servicio");

                entity.Property(e => e.Url)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("Direccion de internet de referencia al servicio");

                entity.HasOne(d => d.DistritoSaludNavigation)
                    .WithMany(p => p.ServicioSigsas)
                    .HasForeignKey(d => d.DistritoSalud)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Servicio_Sigsa_Distrito_Salud");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
