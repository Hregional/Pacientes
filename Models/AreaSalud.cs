using System;
using System.Collections.Generic;

namespace DatosPacientes.Models;

public partial class AreaSalud
{
    /// <summary>
    /// Codigo identificador en la tabla
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Nombre o descripcion del area de salud
    /// </summary>
    public string Nombre { get; set; } = null!;

    /// <summary>
    /// identificador del area de Salud -sigsa
    /// </summary>
    public string Idas { get; set; } = null!;

    /// <summary>
    /// Referencia a la tabla de Departamento
    /// </summary>
    public int Departamento { get; set; }

    /// <summary>
    /// Referencia a la tabla de Region
    /// </summary>
    public int Region { get; set; }

    /// <summary>
    /// Estado del area de salud
    /// </summary>
    public int Estado { get; set; }

    /// <summary>
    /// La direccion de la sede 
    /// </summary>
    public string? DireccionSede { get; set; }

    /// <summary>
    /// telefono de referencia
    /// </summary>
    public string? TelefonoSede { get; set; }

    /// <summary>
    /// persona encargada o coordinador
    /// </summary>
    public string? DirectorArea { get; set; }

    /// <summary>
    /// coordenada altitud
    /// </summary>
    public string? Altitud { get; set; }

    /// <summary>
    /// coordenada latitud
    /// </summary>
    public string? Latitud { get; set; }

    public virtual Departamento DepartamentoNavigation { get; set; } = null!;

    public virtual ICollection<DistritoSalud> DistritoSaluds { get; } = new List<DistritoSalud>();
}
