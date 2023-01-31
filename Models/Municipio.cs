using System;
using System.Collections.Generic;

namespace DatosPacientes.Models;

public partial class Municipio
{
    /// <summary>
    /// Almacena identificador único de Municipio
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Almacena el nombre del Municipio
    /// </summary>
    public string Nombre { get; set; } = null!;

    /// <summary>
    /// referencia al codigo de Municipio segun SIGSA
    /// </summary>
    public int Idmun { get; set; }

    /// <summary>
    /// Hace referencia a la entidad Departamento
    /// </summary>
    public int Departamento { get; set; }

    /// <summary>
    /// guarda si el municipio es cabecera departamental
    /// </summary>
    public bool Cabecera { get; set; }

    public virtual Departamento DepartamentoNavigation { get; set; } = null!;

    public virtual ICollection<Direccion> Direccions { get; } = new List<Direccion>();

    public virtual ICollection<LugarPoblado> LugarPoblados { get; } = new List<LugarPoblado>();
}
