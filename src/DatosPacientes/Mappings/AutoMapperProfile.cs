using AutoMapper;
using DatosPacientes.DTOs;
using DatosPacientes.Models;
using System;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Paciente, PacienteDTO>()
            .ForMember(dest =>
            dest.Archivo_Fisico,
            opt => opt.Ignore());
        CreateMap<Persona, PersonaDTO>()
            .ForMember(dest =>
            dest.Direcciones,
            opt => opt.Ignore());
        CreateMap<Direccion, DireccionDTO>();
     
     }
}
