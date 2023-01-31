using System;
using System.Collections.Generic;

namespace DatosPacientes.Models;

public partial class Nacionalidad
{
    /// <summary>
    /// Almacena identificador único de Nacionalidad
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Almacena la nacionalidad existentes
    /// </summary>
    public string Nombre { get; set; } = null!;

    public virtual ICollection<Paciente> Pacientes { get; } = new List<Paciente>();
}
