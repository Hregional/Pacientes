using Microsoft.EntityFrameworkCore;

namespace DatosPacientes.Models
{
    [Keyless]
    public class PacienteSeleccionarCatalogoPorNoHistoriaClinica
    {

        public int Codigo { get; set; }
        public int Persona { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; }  = string.Empty;
        public string Historia_Clinica { get; set; } = string.Empty;
        public string Edad { get; set; } = string.Empty;
        public string Padre { get; set; } = string.Empty;
        public string Madre { get; set; } = string.Empty;
        public string Procedencia { get; set; } = string.Empty;
        public bool? Archivo_Fisico { get; set; } = false;
    }
}
