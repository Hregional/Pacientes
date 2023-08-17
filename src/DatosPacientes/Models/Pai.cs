using System;
using System.Collections.Generic;

namespace DatosPacientes.Models
{
    public partial class Pai
    {
        public Pai()
        {
            Departamentos = new HashSet<Departamento>();
            Direccions = new HashSet<Direccion>();
        }

        /// <summary>
        /// Almacena el identificador único de Pais
        /// </summary>
        public int Codigo { get; set; }
        /// <summary>
        /// Almacena el nombre del país
        /// </summary>
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Departamento> Departamentos { get; set; }
        public virtual ICollection<Direccion> Direccions { get; set; }
    }
}
