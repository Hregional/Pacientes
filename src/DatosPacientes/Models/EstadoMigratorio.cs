using System;
using System.Collections.Generic;

namespace DatosPacientes.Models
{
    public partial class EstadoMigratorio
    {
        public EstadoMigratorio()
        {
            Pacientes = new HashSet<Paciente>();
        }

        /// <summary>
        /// Identificador único para un Estado Migratorio
        /// </summary>
        public int Codigo { get; set; }
        /// <summary>
        /// Contiene el nombre o descripción de un Estado Migratorio
        /// </summary>
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Paciente> Pacientes { get; set; }
    }
}
