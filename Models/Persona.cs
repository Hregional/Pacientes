using System;
using System.Collections.Generic;

namespace DatosPacientes.Models;

public partial class Persona
{
    /// <summary>
    /// Identificador unico para una persona
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Contiene el  código generado por el sistema para identificar a la persona (obsoleto)
    /// </summary>
    public string? CodigoIxchel { get; set; }

    /// <summary>
    /// Contiene el primer nombre de la persona
    /// </summary>
    public string Nombre1 { get; set; } = null!;

    /// <summary>
    /// Contiene el segundo nombre de la persona
    /// </summary>
    public string? Nombre2 { get; set; }

    /// <summary>
    /// Contiene el primer apellido de la persona
    /// </summary>
    public string? Apellido1 { get; set; }

    /// <summary>
    /// Contiene el segundo apellido de la persona
    /// </summary>
    public string? Apellido2 { get; set; }

    /// <summary>
    /// Contiene el Apellido de casada
    /// </summary>
    public string? Apellido3 { get; set; }

    /// <summary>
    /// Contiene la direccion de la persona individual o juridica o bien de la agencia
    /// </summary>
    public int? Direccion { get; set; }

    /// <summary>
    /// Contiene el telefono de la persona
    /// </summary>
    public string? Telefono1 { get; set; }

    /// <summary>
    /// Contienen el segundo telefono de la persona en caso que el primer telefono no funcionara
    /// </summary>
    public string? Telefono2 { get; set; }

    /// <summary>
    /// Contiene el numero de telefono movil de la persona
    /// </summary>
    public string? Movil { get; set; }

    /// <summary>
    /// Contiene el numero de identificacion tributaria de la persona
    /// </summary>
    public string? Nit { get; set; }

    /// <summary>
    /// Contiene el correo electronico de la persona
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Contiene el nombre de la razon social
    /// </summary>
    public string? RazonSocial { get; set; }

    /// <summary>
    /// Contiene el sexo de la persona
    /// </summary>
    public string? Sexo { get; set; }

    /// <summary>
    /// Contiene la fecha de nacimiento de la persona
    /// </summary>
    public DateTime? FechaNacimiento { get; set; }

    /// <summary>
    /// Contiene la edad de la persona en años
    /// </summary>
    public int? EdadAnios { get; set; }

    /// <summary>
    /// Contiene la edad de la persona en meses
    /// </summary>
    public int? EdadMeses { get; set; }

    /// <summary>
    /// Contiene la edad de la persona en dias
    /// </summary>
    public int? EdadDias { get; set; }

    /// <summary>
    /// Contiene el estado civil de la persona, ya sea casado o soltero
    /// </summary>
    public byte? EstadoCivil { get; set; }

    /// <summary>
    /// Contiene el nombre completo de la persona
    /// </summary>
    public string? NombreCompleto { get; set; }

    /// <summary>
    /// Contiene observaciones sobre la persona
    /// </summary>
    public string? Observaciones { get; set; }

    /// <summary>
    /// Contiene el estado en que se encuenta la persona siendo 1 activo, 0 inactivo
    /// </summary>
    public int Estado { get; set; }

    /// <summary>
    /// Contiene el Id de persona
    /// </summary>
    public string? CodigoRenap { get; set; }

    /// <summary>
    /// Determina si es persona individual o juridica
    /// </summary>
    public bool? PersonaJuridica { get; set; }

    /// <summary>
    /// Almacena la fecha de muerte de la persona
    /// </summary>
    public DateTime? FechaDefuncion { get; set; }

    /// <summary>
    /// Guarda la fecha de registro de la persona
    /// </summary>
    public DateTime? FechaRegistro { get; set; }

    public virtual Direccion? DireccionNavigation { get; set; }

    public virtual ICollection<Paciente> Pacientes { get; } = new List<Paciente>();
}
