using System;
using System.Collections.Generic;

namespace DatosPacientes.Models;

public partial class DistritoSalud
{
    /// <summary>
    /// Codigo identificador en la tabla
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Nombre o descripcion del distrito
    /// </summary>
    public string Nombre { get; set; } = null!;

    /// <summary>
    /// identificador del Distrito de Salud -sigsa
    /// </summary>
    public string? Idds { get; set; }

    /// <summary>
    /// identificador del area de Salud -sigsa
    /// </summary>
    public string? Idas { get; set; }

    /// <summary>
    /// Referencia a la tabla de area_salud
    /// </summary>
    public int AreaSalud { get; set; }

    /// <summary>
    /// Referencia a la tabla de Departamento
    /// </summary>
    public int? Departamento { get; set; }

    /// <summary>
    /// Referencia a la tabla de Municipio
    /// </summary>
    public int? Municipio { get; set; }

    /// <summary>
    /// Estado del distrito
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
    public string? Coordinador { get; set; }

    /// <summary>
    /// coordenada altitud
    /// </summary>
    public string? Altitud { get; set; }

    /// <summary>
    /// coordenada latitud
    /// </summary>
    public string? Latitud { get; set; }

    public virtual AreaSalud AreaSaludNavigation { get; set; } = null!;

    public virtual ICollection<ServicioSigsa> ServicioSigsas { get; } = new List<ServicioSigsa>();
}
