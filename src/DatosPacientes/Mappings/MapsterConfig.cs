using Mapster;
using DatosPacientes.DTOs;
using DatosPacientes.Models;

namespace DatosPacientes.Mappings
{
    public class MapsterConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Paciente, PacienteDTO>()
                .Ignore(dest => dest.Archivo_Fisico)
                .Ignore(dest => dest.PersonasLink);

            config.NewConfig<Persona, PersonaDTO>()
                .Ignore(dest => dest.Direcciones);

            // Direccion a DireccionDTO se mapea automáticamente si los nombres coinciden
        }
    }
}
