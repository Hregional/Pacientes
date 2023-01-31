using System;
using System.Collections.Generic;

namespace DatosPacientes.Models;

public partial class Pai
{
    /// <summary>
    /// Almacena el identificador único de Pais
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Almacena el nombre del país
    /// </summary>
    public string Nombre { get; set; } = null!;

    public virtual ICollection<Departamento> Departamentos { get; } = new List<Departamento>();

    public virtual ICollection<Direccion> Direccions { get; } = new List<Direccion>();
}
