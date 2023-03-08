using System;
using System.Collections.Generic;

namespace DatosPacientes.Models
{
    public partial class Nacionalidad
    {
        public Nacionalidad()
        {
            Pacientes = new HashSet<Paciente>();
        }

        /// <summary>
        /// Almacena identificador único de Nacionalidad
        /// </summary>
        public int Codigo { get; set; }
        /// <summary>
        /// Almacena la nacionalidad existentes
        /// </summary>
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Paciente> Pacientes { get; set; }
    }
}
