namespace DatosPacientes.DTOs
{
    public class PersonaDTO
    {
        public int Codigo { get; set; }
        
        public int? Direccion { get; set; }
        
        public string? Telefono1 { get; set; }
       
        public string? Sexo { get; set; }
        
        public DateTime? FechaNacimiento { get; set; }
        
        public byte? EstadoCivil { get; set; }
       
        public string? NombreCompleto { get; set; }
        
        public string? CodigoRenap { get; set; }

        public DateTime? FechaDefuncion { get; set; }
        
        public ICollection<PacienteDTO> Pacientes { get; set; }
    }
}
