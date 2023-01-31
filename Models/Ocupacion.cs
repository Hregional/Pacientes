using System;
using System.Collections.Generic;

namespace DatosPacientes.Models;

public partial class Ocupacion
{
    /// <summary>
    /// Identificador único para una ocupación
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Contiene el nombre o descripción de la ocupación
    /// </summary>
    public string Nombre { get; set; } = null!;

    public virtual ICollection<Paciente> Pacientes { get; } = new List<Paciente>();
}
