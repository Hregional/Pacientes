using AutoMapper;
using DatosPacientes.DTOs;
using DatosPacientes.Models;
using System;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Paciente, PacienteDTO>();
        CreateMap<Persona, PersonaDTO>();
        CreateMap<Direccion, DireccionDTO>();
     
     }
}
