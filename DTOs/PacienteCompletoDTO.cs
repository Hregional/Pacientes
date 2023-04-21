using System.ComponentModel.DataAnnotations;

namespace DatosPacientes.DTOs
{
    public class PacienteCompletoDTO
    {
        public int Codigo { get; set; }
        public int Persona { get; set; }
        public string? NoHistoriaClinica { get; set; } = string.Empty;
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaNacimiento { get; set; }
        public string? Nombre_Resposable { get; set; }
        public string? Direccion_Responsable { get; set; }
        public string? Telefono_Responsable { get; set; }
        public string? NombrePadre { get; set; }
        public string? NombreMadre { get; set; }
        public string? LugarNacimiento { get; set; }
        public bool? Archivo_Fisico { get; set; }

        
    }
}
