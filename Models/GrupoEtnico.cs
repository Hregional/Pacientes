using System;
using System.Collections.Generic;

namespace DatosPacientes.Models;

public partial class GrupoEtnico
{
    /// <summary>
    /// Identificador único para el grupo étnico
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Contiene el nombre o descripción del Grupo Étnico
    /// </summary>
    public string Nombre { get; set; } = null!;

    public virtual ICollection<Paciente> Pacientes { get; } = new List<Paciente>();
}
