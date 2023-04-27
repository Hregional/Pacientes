using System;
using System.Collections.Generic;

namespace DatosPacientes.Models
{
    public partial class Direccion
    {
        public Direccion()
        {
            Personas = new HashSet<Persona>();
        }

        /// <summary>
        /// Este es el identificador unico de la tabla direccion
        /// </summary>
        public int Codigo { get; set; }
        /// <summary>
        /// Almacena la descripción de la dirección
        /// </summary>
        public string? Descripcion { get; set; }
        /// <summary>
        /// Almacena la zona en donde vive
        /// </summary>
        public string? Zona { get; set; }
        /// <summary>
        /// Almacena la colonia donde vive
        /// </summary>
        public string? Colonia { get; set; }
        /// <summary>
        /// Hace referencia a la entidad Comunidad
        /// </summary>
        public int? Comunidad { get; set; }
        /// <summary>
        /// Hace referencia a la entidad Lugar Poblado
        /// </summary>
        public int? LugarPoblado { get; set; }
        /// <summary>
        /// Hace referencia a la entidad Municipio
        /// </summary>
        public int? Municipio { get; set; }
        /// <summary>
        /// Hace referencia a la entidad Departamento
        /// </summary>
        public int? Departamento { get; set; }
        /// <summary>
        /// Hace referencia a la entidad Pais
        /// </summary>
        public int? Pais { get; set; }
        /// <summary>
        /// Almacena la dirección completa en donde vive
        /// </summary>
        public string? DireccionCompleta { get; set; }
        /// <summary>
        /// Almacena el código postal de la persona
        /// </summary>
        public string? CodigoPostal { get; set; }
        /// <summary>
        /// Almacena la referencia de donde vive
        /// </summary>
        public string? Referencia { get; set; }

        public virtual Comunidad? ComunidadNavigation { get; set; }
        public virtual Departamento? DepartamentoNavigation { get; set; }
        public virtual LugarPoblado? LugarPobladoNavigation { get; set; }
        public virtual Municipio? MunicipioNavigation { get; set; }
        public virtual Pai? PaisNavigation { get; set; }
        public virtual ICollection<Persona> Personas { get; set; }
    }
}
