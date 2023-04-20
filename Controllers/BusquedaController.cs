using DatosPacientes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using DatosPacientes.Models.SP;
using AutoMapper;
using DatosPacientes.DTOs;
using AutoMapper.QueryableExtensions;

namespace DatosPacientes.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    [ApiController]
    [Route("api/[controller]")]
    public class BusquedaController : ControllerBase
    {
        private readonly RecepcionV2Context _context;
        private readonly IMapper _mapper;

        public BusquedaController(RecepcionV2Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("persona")]
        public async Task<List<PacienteCompletoDTO>> getPersonas(string NoHistoriaClinica, int pageNumber = 1, int pageSize = 100)
        {
            var query = _context.Pacientes
            .Join(_context.Personas, p => p.Persona, per => per.Codigo, (p, per) => new { Paciente = p, Persona = per })
            .Where(a => (a.Paciente.NoHistoriaClinica.Contains(NoHistoriaClinica) || NoHistoriaClinica == "-1"))
            .OrderBy(a => a.Persona.Nombre1)
            .Select(a => new PacienteCompletoDTO()
            {
                Codigo = a.Paciente.Codigo,
                Persona = a.Paciente.Persona,
                Nombres = a.Persona.Nombre1 + " " + (a.Persona.Nombre2 ?? ""),
                Apellidos = a.Persona.Apellido1 + " " + (a.Persona.Apellido2 ?? ""),
                NoHistoriaClinica = a.Paciente.NoHistoriaClinica,
                FechaNacimiento = a.Persona.FechaNacimiento,
                nombrePadre = a.Paciente.NombrePadre,
                nombreMadre = a.Paciente.NombreMadre,
                lugarNacimiento = a.Paciente.LugarNacimiento,
                Archivo_Fisico = a.Paciente.ArchivoFisico
            });

            return query.ToList();

 

        }



        [HttpGet("paciente/{NoHistoriaClinica}")]
         public async Task<List<PacienteDTO>> GetPatients(string NoHistoriaClinica) {

            var pacientes = await _context.Pacientes.
                 Where(p => p.NoHistoriaClinica.Contains(NoHistoriaClinica))
                 .ToListAsync();

             var pacienteDto = _mapper.Map<List<PacienteDTO>> (pacientes);

             return pacienteDto;

         }
        [HttpGet("pacientePaged")]
        public async Task<List<PacienteDTO>> GetPatients(string NoHistoriaClinica, int pageNumber = 1, int pageSize = 10)
        {
            var pacientes = await _context.Pacientes
                .Where(p => p.NoHistoriaClinica.Contains(NoHistoriaClinica))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pacientesDto = _mapper.Map<List<PacienteDTO>>(pacientes);

            return pacientesDto;
        }

        [HttpGet("pacientes")]
        public async Task<List<PacienteDTO>> GetAllPatients(int pageNumber = 1, int pageSize = 10)
        {
            var pacientes = await _context.Pacientes.OrderByDescending(p => p.Codigo)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();

            var PacienteDto = _mapper.Map<List<PacienteDTO>>(pacientes);

            return PacienteDto;

        }
    }
}
