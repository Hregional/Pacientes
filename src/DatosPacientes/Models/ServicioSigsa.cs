using System;
using System.Collections.Generic;

namespace DatosPacientes.Models
{
    public partial class ServicioSigsa
    {
        /// <summary>
        /// Codigo identificador de la tupla
        /// </summary>
        public int Codigo { get; set; }
        /// <summary>
        /// Nombre o descripcion del servicio
        /// </summary>
        public string Nombre { get; set; } = null!;
        /// <summary>
        /// referencia al codigo de Servicio -SIGSA
        /// </summary>
        public string Idts { get; set; } = null!;
        /// <summary>
        /// Referencia al codigo de distrito -SIGSA
        /// </summary>
        public string? Idds { get; set; }
        /// <summary>
        /// referencia al codigo de area -SIGSA
        /// </summary>
        public string? Idas { get; set; }
        /// <summary>
        /// referencia a la tabla de Distrito_Salud
        /// </summary>
        public int DistritoSalud { get; set; }
        /// <summary>
        /// referencia a la tabla de Tipo_Servicio
        /// </summary>
        public int TipoServicio { get; set; }
        /// <summary>
        /// Determina el estado de la tupla en la tabla eliminado/activo
        /// </summary>
        public int Estado { get; set; }
        /// <summary>
        /// Direccion fisica del servicio
        /// </summary>
        public string? Direccion { get; set; }
        /// <summary>
        /// telefono de contacto del servicio
        /// </summary>
        public string? Telefono { get; set; }
        /// <summary>
        /// Responsable del servicio
        /// </summary>
        public string? Responsable { get; set; }
        /// <summary>
        /// coordenada altitud
        /// </summary>
        public string? Altitud { get; set; }
        /// <summary>
        /// coordenada latitud
        /// </summary>
        public string? Latitud { get; set; }
        /// <summary>
        /// identificacion de jurisdiccion mixta
        /// </summary>
        public int? JurisdiccionMixta { get; set; }
        /// <summary>
        /// codigo de empleado
        /// </summary>
        public int? Empleado { get; set; }
        /// <summary>
        /// email del empleado del servico
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Direccion de internet de referencia al servicio
        /// </summary>
        public string? Url { get; set; }

        public virtual DistritoSalud DistritoSaludNavigation { get; set; } = null!;
    }
}
