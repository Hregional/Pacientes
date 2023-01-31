using System;
using System.Collections.Generic;

namespace DatosPacientes.Models;

public partial class Comunidad
{
    /// <summary>
    /// Almacena identificador único de comunidad
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Almacena el nombre de comunidad
    /// </summary>
    public string Nombre { get; set; } = null!;

    /// <summary>
    /// codigo identificador del centro comunidad -SIGSA
    /// </summary>
    public string? Idc { get; set; }

    /// <summary>
    /// codigo identificador del centro comunitario -SIGSA
    /// </summary>
    public string? Idcc { get; set; }

    /// <summary>
    /// referencia al codigo de Servicio -SIGSA
    /// </summary>
    public string? Idts { get; set; }

    /// <summary>
    /// Referencia al codigo de distrito -SIGSA
    /// </summary>
    public string? Idds { get; set; }

    /// <summary>
    /// referencia al codigo de area -SIGSA
    /// </summary>
    public string? Idas { get; set; }

    /// <summary>
    /// referencia a la tabla de Centro_Comunitario
    /// </summary>
    public int CentroComunitario { get; set; }

    /// <summary>
    /// Hace referencia a la entidad Lugar_Poblado
    /// </summary>
    public int LugarPoblado { get; set; }

    /// <summary>
    /// referencia a la tabla de Municipio
    /// </summary>
    public int Municipio { get; set; }

    /// <summary>
    /// referencia a la tabla de Departamento
    /// </summary>
    public int Departamento { get; set; }

    /// <summary>
    /// Categoria de la comunidad
    /// </summary>
    public string? Categoria { get; set; }

    /// <summary>
    /// referencia SIGSA
    /// </summary>
    public int? PuntoConvergencia { get; set; }

    /// <summary>
    /// Estado de la tupla
    /// </summary>
    public int? Estado { get; set; }

    /// <summary>
    /// referencia SIGSA
    /// </summary>
    public int? Poblacion { get; set; }

    /// <summary>
    /// referencia SIGSA
    /// </summary>
    public int? DistanciaKm { get; set; }

    /// <summary>
    /// coordenada Altitud
    /// </summary>
    public string? Altitud { get; set; }

    /// <summary>
    /// coordenada Latitud
    /// </summary>
    public string? Latitud { get; set; }

    public virtual ICollection<Direccion> Direccions { get; } = new List<Direccion>();

    public virtual LugarPoblado LugarPobladoNavigation { get; set; } = null!;
}
