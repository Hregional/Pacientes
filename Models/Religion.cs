using System;
using System.Collections.Generic;

namespace DatosPacientes.Models
{
    public partial class Religion
    {
        public Religion()
        {
            Pacientes = new HashSet<Paciente>();
        }

        /// <summary>
        /// Identificador único para una religión
        /// </summary>
        public int Codigo { get; set; }
        /// <summary>
        /// Contiene el nombre o descripción de la religión
        /// </summary>
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Paciente> Pacientes { get; set; }
    }
}
