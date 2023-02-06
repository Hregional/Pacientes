using System;
using System.Collections.Generic;

namespace DatosPacientes.Models
{
    public partial class Departamento
    {
        public Departamento()
        {
            AreaSaluds = new HashSet<AreaSalud>();
            Direccions = new HashSet<Direccion>();
            Municipios = new HashSet<Municipio>();
        }

        /// <summary>
        /// Almacena el identificador único de Departamento
        /// </summary>
        public int Codigo { get; set; }
        /// <summary>
        /// Almacena el nombre del Deparamento
        /// </summary>
        public string Nombre { get; set; } = null!;
        /// <summary>
        /// Hace referencia a la entidad Pais
        /// </summary>
        public int Pais { get; set; }

        public virtual Pai PaisNavigation { get; set; } = null!;
        public virtual ICollection<AreaSalud> AreaSaluds { get; set; }
        public virtual ICollection<Direccion> Direccions { get; set; }
        public virtual ICollection<Municipio> Municipios { get; set; }
    }
}
