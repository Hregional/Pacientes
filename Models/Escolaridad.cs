using System;
using System.Collections.Generic;

namespace DatosPacientes.Models;

public partial class Escolaridad
{
    /// <summary>
    /// Identificador único de Escolaridad
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Almacena el nombre de escolaridad
    /// </summary>
    public string Nombre { get; set; } = null!;

    public virtual ICollection<Paciente> Pacientes { get; } = new List<Paciente>();
}
