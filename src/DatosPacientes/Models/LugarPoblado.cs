using System;
using System.Collections.Generic;

namespace DatosPacientes.Models
{
    public partial class LugarPoblado
    {
        public LugarPoblado()
        {
            Comunidads = new HashSet<Comunidad>();
            Direccions = new HashSet<Direccion>();
        }

        /// <summary>
        /// Codigo identificador de la tupla
        /// </summary>
        public int Codigo { get; set; }
        /// <summary>
        /// Nombre o descripcion del lugar_poblado
        /// </summary>
        public string Nombre { get; set; } = null!;
        /// <summary>
        /// referencia al codigo de lugar poblado-SIGSA
        /// </summary>
        public int Idlp { get; set; }
        /// <summary>
        /// referencia a la tabla de Municipio
        /// </summary>
        public int Municipio { get; set; }
        /// <summary>
        /// referencia a la tabla de Municipio
        /// </summary>
        public int Departamento { get; set; }
        /// <summary>
        /// Categoria en que se clasifica el lugar poblado
        /// </summary>
        public string? Categoria { get; set; }
        /// <summary>
        /// Categoria en que se clasifica el lugar poblado ine2002
        /// </summary>
        public int? Ine2002 { get; set; }
        /// <summary>
        /// Categoria en que se clasifica el lugar poblado ine2004
        /// </summary>
        public int? Ine2004 { get; set; }
        /// <summary>
        /// Categoria en que se clasifica el lugar poblado ine2004
        /// </summary>
        public string? Categoria2004 { get; set; }

        public virtual Municipio MunicipioNavigation { get; set; } = null!;
        public virtual ICollection<Comunidad> Comunidads { get; set; }
        public virtual ICollection<Direccion> Direccions { get; set; }
    }
}
