﻿using System;
using System.Collections.Generic;

namespace DatosPacientes.Models;

public partial class Discapacidad
{
    /// <summary>
    /// Identificador único para una discapacidad
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Contiene el nombre o descripción de la discapacidad
    /// </summary>
    public string Nombre { get; set; } = null!;

    public virtual ICollection<Paciente> Pacientes { get; } = new List<Paciente>();
}
