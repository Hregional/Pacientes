using System;
using System.Collections.Generic;

namespace DatosPacientes.Models;

public partial class FuenteRegistro
{
    /// <summary>
    /// Almacena el codigo de Fuente de Ingreso
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Almacena el Nombre de Fuente de Ingreso
    /// </summary>
    public string Nombre { get; set; } = null!;

    public virtual ICollection<Paciente> Pacientes { get; } = new List<Paciente>();
}
