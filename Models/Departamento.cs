using System;
using System.Collections.Generic;

namespace DatosPacientes.Models;

public partial class Departamento
{
    /// <summary>
    /// Almacena el identificador único de Departamento
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Almacena el nombre del Deparamento
    /// </summary>
    public string Nombre { get; set; } = null!;

    /// <summary>
    /// Hace referencia a la entidad Pais
    /// </summary>
    public int Pais { get; set; }

    public virtual ICollection<AreaSalud> AreaSaluds { get; } = new List<AreaSalud>();

    public virtual ICollection<Direccion> Direccions { get; } = new List<Direccion>();

    public virtual ICollection<Municipio> Municipios { get; } = new List<Municipio>();

    public virtual Pai PaisNavigation { get; set; } = null!;
}
