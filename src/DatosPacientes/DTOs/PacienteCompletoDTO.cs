using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DatosPacientes.DTOs
{
    public class PacienteCompletoDTO
    {
        public int Codigo { get; set; }
        public int Persona { get; set; }
        public string? NoHistoriaClinica { get; set; } = string.Empty;

        public string? CodigoRenap { get; set; }

        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaNacimiento { get; set; }

        private string? _sexo;
        public string? Sexo
        {
            get { return _sexo; }
            set { _sexo = (value == "0") ? "M" : "F"; }
        }
        public string? CodigoRenap { get; set; }
        public string? Edad { get; set; }
        public string? Nombre_Resposable { get; set; }
        public string? Direccion_Responsable { get; set; }
        public string? Telefono_Responsable { get; set; }
        public string? NombreMadre { get; set; }
        public string? NombrePadre { get; set; }
        public string? LugarNacimiento { get; set; }
        public bool? Archivo_Fisico { get; set; }
        public DireccionPacienteDTO? DireccionPaciente { get; set; }
    }

    public class DireccionPacienteDTO
    {
        public string? Descripcion { get; set; }
        public string? Municipio { get; set; }
        public string? Departamento { get; set; }
    }
}
