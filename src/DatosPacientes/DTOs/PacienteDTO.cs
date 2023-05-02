namespace DatosPacientes.DTOs
{
    public class PacienteDTO
    {
        public int Codigo { get; set; }
        public int Persona { get; set; }
        public string? NombreCompleto { get; set; }
        public string? NombrePadre { get; set; }
        public string? NombreMadre { get; set; }
        public string? NoHistoriaClinica { get; set; } = string.Empty;
        public string? LugarNacimiento { get; set; }
        public bool? Archivo_Fisico { get; set; }
        public string PersonasLink { get; set; } = string.Empty;
    }
}
