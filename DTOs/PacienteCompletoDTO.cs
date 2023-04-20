namespace DatosPacientes.DTOs
{
    public class PacienteCompletoDTO
    {
        public int Codigo { get; set; }
        public int Persona { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string nombrePadre { get; set; }
        public string nombreMadre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? NoHistoriaClinica { get; set; } = string.Empty;
        public string lugarNacimiento { get; set; }
        public bool? Archivo_Fisico { get; set; }
    }
}
