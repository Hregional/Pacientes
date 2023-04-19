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
        public async Task<List<PersonaDTO>> getPersonas(int pageNumber = 1, int pageSize = 100)
        {
            var personas = await _context.Personas
                .Include(p => p.Pacientes)
                //.Where(p => p.Pacientes.Any(pa => pa.NoHistoriaClinica.Contains("3574")))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                //.ProjectTo<PersonaDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var pacienteDto = _mapper.Map<List<PersonaDTO>>(personas);

            return pacienteDto;

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
