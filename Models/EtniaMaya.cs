using System;
using System.Collections.Generic;

namespace DatosPacientes.Models;

public partial class EtniaMaya
{
    /// <summary>
    /// Almacena el codigo de Etnia_Maya
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Almacena el nombre de la sub_Grupo_Etnico
    /// </summary>
    public string Nombre { get; set; } = null!;

    public virtual ICollection<Paciente> Pacientes { get; } = new List<Paciente>();
}
