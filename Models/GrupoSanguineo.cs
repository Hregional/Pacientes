using System;
using System.Collections.Generic;

namespace DatosPacientes.Models;

public partial class GrupoSanguineo
{
    /// <summary>
    /// Almacena el Codigo de Grupo Sanguineo
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Almacena el Nombre de Grupo Sanguineo
    /// </summary>
    public string Nombre { get; set; } = null!;
}
